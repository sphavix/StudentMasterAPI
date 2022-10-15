using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMasterAPI.Models.DTOs;
using StudentMasterAPI.Services;

namespace StudentMasterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _service;
        public StudentsController(IStudentService service)
        {
            _service = service;
        }


        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentDTO student)
        {
            try
            {
                var response = await _service.AddStudent(student);

                return Ok(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
