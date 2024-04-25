
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs.Authentication.Login
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Pleace enter your user name or email")]
        public string UserNameOrEmail { get; set; } = null!;

        [Required(ErrorMessage ="Please enter your password")]
        public string Password { get; set; } = null!;
    }
}
