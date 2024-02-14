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
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        // GET: Students
        private ErrorLogs _logger = new ErrorLogs();
        private StudentsUtils _studentsUtils = new StudentsUtils();


        [Route("")]
        [HttpGet]
        public IHttpActionResult getAll()
        {
            try
            {
                var result = _studentsUtils.getAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }


        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult getSingleStudent(int? id)
        {
            try
            {
                var result = _studentsUtils.getById(id);

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
        public IHttpActionResult create([FromBody] StudentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                _studentsUtils.createStudent(dto);

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
        public IHttpActionResult update(int id, [FromBody] StudentCreateDTO dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(s => s.ErrorMessage).FirstOrDefault());

            try
            {
                _studentsUtils.studentUpdate(dto, id);
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.createLogs(ex);
                return InternalServerError(ex);
            }
        }

    }
}