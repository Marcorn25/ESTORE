using System.ComponentModel.DataAnnotations;

namespace ESTORE.Shared.DTO.User
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Fullname is a required field")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is a required field")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]+$",
                   ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email Address is a required field")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Username is a required field")]
        public string UserName { get; set; } = string.Empty;
    }
}
