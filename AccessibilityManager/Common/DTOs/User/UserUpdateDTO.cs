using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.User
{
    public class UserUpdateDTO
    {

        [MinLength(2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [MaxLength(50, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string? FirstName { get; set; } 

        [MinLength(2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        [MaxLength(50, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(2, ErrorMessage = "Username must be between 2 and 50 characters.")]
        [MaxLength(50, ErrorMessage = "Username must be between 2 and 50 characters.")]
        public string? Username { get; set; } 
    }
}
