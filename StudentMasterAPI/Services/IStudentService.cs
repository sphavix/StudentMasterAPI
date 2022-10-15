using StudentMasterAPI.Models;
using StudentMasterAPI.Models.Common;
using StudentMasterAPI.Models.DTOs;

namespace StudentMasterAPI.Services
{
    public interface IStudentService
    {
        Task<MainResponse> GetAllStudents();
        //Task<IEnumerable<StudentDTO>> GetAllStudents();
        Task<MainResponse> GetStudentById(int id);
        //Task<StudentDTO> GetStudentById(int id);
        Task<MainResponse> AddStudent(StudentDTO student);
        Task<MainResponse> UpdateStudent(UpdateStudentDTO student);
        Task<MainResponse> DeleteStudent(int id);
    }
}
