using System.ComponentModel.DataAnnotations;

namespace FACL_Locker_Room_API.DTO
{
    public class AccountDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;

        [Required]
        public string Nationality { get; set; }
    }
}
