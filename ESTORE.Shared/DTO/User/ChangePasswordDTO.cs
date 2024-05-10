using System.ComponentModel.DataAnnotations;

namespace ESTORE.Shared.DTO.User
{
    public class ChangePasswordDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Password is a required field")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]+$",
                      ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one digit")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is a required field")]
        [Compare(nameof(Password), ErrorMessage = "Password don't Match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
