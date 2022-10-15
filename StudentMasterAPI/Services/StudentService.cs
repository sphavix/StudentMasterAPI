using StudentMasterAPI.Data;
using StudentMasterAPI.Models;
using StudentMasterAPI.Models.Common;
using StudentMasterAPI.Models.DTOs;

namespace StudentMasterAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentMasterDbContext _context;

        public StudentService(StudentMasterDbContext context)
        {
            _context = context;
        }

        public async Task<MainResponse> AddStudent(StudentDTO student)
        {
            var response = new  MainResponse();

            try
            {
                //Check if is there any student in the database with the same email address.
                if(_context.Students.Any(x => x.Email.ToLower() == student.Email.ToLower()))
                {
                    response.ErrorMessage = "Student with this email already exist";
                    response.IsSuccess = false;
                }
                else
                {
                    //Add new student record
                    await _context.Students.AddAsync(new Student
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        Gender = student.Gender,
                        Address = student.Address,
                    });
                    await _context.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.ErrorMessage = "Student Successfully Added!";
                }
            }
            catch(Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
