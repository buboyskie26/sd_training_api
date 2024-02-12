using api.App_Utility;
using api.App_Utility.Data;
using api.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
/*using System.Web.Mvc;*/

namespace api.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthenticationController : ApiController
    {
        private ErrorLogs _logger = new ErrorLogs();
        private UsersUtils _userUtils = new UsersUtils();

        [HttpPost]
        [Route("signin")]
        public IHttpActionResult Authenticate([FromBody] UsersSigninDTO dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                var user = _userUtils.signin(dto);

                if (user == null)
                    return BadRequest("Invalid Username and password");

                if (user.IsActive == false)
                    return BadRequest("Your account is inactive, Please contact your adminstrator.");

                string result = _userUtils.getToken(user.Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.createLogs(ex, JsonConvert.SerializeObject(dto));
                return InternalServerError(ex);
            }
        }

    }
}