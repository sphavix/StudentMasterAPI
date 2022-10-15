using StudentMasterAPI.Models.Common;
using StudentMasterAPI.Models.DTOs;

namespace StudentMasterAPI.Services
{
    public interface IStudentService
    {

        Task<MainResponse> AddStudent(StudentDTO student);
    }
}
