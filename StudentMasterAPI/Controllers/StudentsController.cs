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

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _service.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _service.GetStudentById(id);
            return Ok(student);
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

        [HttpPut("UpdateStudent/{id}")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDTO student)
        {
            try
            {
                //Check if is not in the databaste
                if(student.StudentId > 0)
                {
                    var response = await _service.UpdateStudent(student);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Error Occured, please contact your system administrator.");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var response = await _service.DeleteStudent(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
