using api.App_Utility;
using api.App_Utility.Data;
using api.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace api.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        // GET: User

        private ErrorLogs _logger = new ErrorLogs();
        private UsersUtils _userUtils = new UsersUtils();



     /*   [Authorization]*/
        [Route("profile")]
        [HttpGet]
        public IHttpActionResult getProfile()
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(Request).ReadValue("userId"));

                var result  = _userUtils.getById(user_id);

                if (result == null)
                {
                    return BadRequest("User not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }


        [Route("")]
        [HttpGet]
        public IHttpActionResult getAll()
        {
            try
            {
                var result = _userUtils.getAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }


        [Route("")]
        [HttpPost]
        public IHttpActionResult create([FromBody] UsersCreateDTO dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(Request).ReadValue("userId"));

                _userUtils.create(dto, user_id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }

        [Route("{id:int}")]
        [HttpPut]
        public IHttpActionResult update(int id, [FromBody] UsersUpdateDTO dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userId"));
                var user = _userUtils.getById(id);

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                if(user.Email != dto.email)
                {
                    var emailValidate = _userUtils.validateEmail(dto.email);
                    if (emailValidate != "")
                    {
                        return BadRequest(emailValidate);
                    }
                }
                _userUtils.update(dto, id, user_id);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }


        [Route("change/status/{id}")]
        [HttpPut]
        public IHttpActionResult changeStaus(int id, [FromBody] ChangeStatusDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                int user_id = Convert.ToInt32(new AuthTokenParser(this.Request).ReadValue("userId"));

                var user = _userUtils.getById(id);

                if(user == null)
                {
                    return BadRequest("User not found");
                }

                _userUtils.changeStatus(id, user_id, dto.is_active);

                return Ok();

            }catch(Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }
    }
}