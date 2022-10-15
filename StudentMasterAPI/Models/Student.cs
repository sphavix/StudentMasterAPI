using System.ComponentModel.DataAnnotations;

namespace StudentMasterAPI.Models
{
    public class Student
    {
        [Key]
        public int StudenId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string  LastName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(5)]
        public string Gender { get; set; }

        public string? Address { get; set; }
    }
}
