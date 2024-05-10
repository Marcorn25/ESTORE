using System.ComponentModel.DataAnnotations;

namespace ESTORE.Shared.DTO.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email or Username is a required field")]
        public string EmailOrUsername { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is a required field")]
        public string Password { get; set; } = string.Empty;
    }
}
