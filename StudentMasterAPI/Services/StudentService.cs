using Microsoft.EntityFrameworkCore;
using StudentMasterAPI.Data;
using StudentMasterAPI.Models;
using StudentMasterAPI.Models.Common;
using StudentMasterAPI.Models.DTOs;
using System.Net;
using System.Reflection;

namespace StudentMasterAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentMasterDbContext _context;

        public StudentService(StudentMasterDbContext context)
        {
            _context = context;
        }

        public async Task<MainResponse> GetAllStudents()
        {
            var response = new MainResponse();
            try
            {
                response.Content = await _context.Students.ToListAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }



        //We can also use the method above to get content and uncomment the method on the interface. 
        //public async Task<IEnumerable<StudentDTO>> GetAllStudents() //Get content using LINQ to Query
        //{
        //    var students = await _context.Students.Select(x => new StudentDTO
        //    {
        //        StudentId = x.StudenId,
        //        FirstName = x.FirstName,
        //        LastName = x.LastName,
        //        Email = x.Email,
        //        Gender = x.Gender,
        //        Address = x.Address
        //    }).ToListAsync();
        //    return students;
        //}

        public async Task<MainResponse> GetStudentById(int id)
        {
            var response = new MainResponse();
            try
            {
                response.Content = await _context.Students.Where(x => x.StudenId == id).FirstOrDefaultAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }


        //We can also use the method above to get content and uncomment the method on the interface. 
        //public async Task<StudentDTO> GetStudentById(int id)//Get content using LINQ to Query
        //{
        //    var student = await _context.Students.Select(x => new StudentDTO
        //    {
        //        StudentId = x.StudenId,
        //        FirstName = x.FirstName,
        //        LastName = x.LastName,
        //        Email = x.Email,
        //        Gender = x.Gender,
        //        Address = x.Address
        //    }).Where(x => x.StudentId == id).FirstOrDefaultAsync();
        //    return student;
        //}

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

        public async Task<MainResponse> UpdateStudent(UpdateStudentDTO student)
        {
            var response = new MainResponse();

            try
            {
                //Check if is there any student exist.
                var studentExist = _context.Students.Where(x => x.StudenId == student.StudentId).FirstOrDefault();
                if(studentExist != null)
                {
                    //student.StudentId = student.StudentId;
                    studentExist.FirstName = student.FirstName;
                    studentExist.LastName = student.LastName;
                    studentExist.Email = student.Email;
                    studentExist.Address = student.Address;

                    await _context.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.ErrorMessage = "Student Successfully Updated!";
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Student Not Found";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }
    
        public async Task<MainResponse> DeleteStudent(int id)
        {
            var response = new MainResponse();
            try
            {
                //Check if student to be deleted exist in the database.
                var student = await _context.Students.Where(x => x.StudenId == id).FirstOrDefaultAsync();
                if(student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.ErrorMessage = "Student Successfully Deleted!";
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Student Not Found";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
    
    }


}
