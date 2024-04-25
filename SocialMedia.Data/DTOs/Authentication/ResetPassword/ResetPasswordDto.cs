
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs.Authentication.ResetPassword
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "The password and conformation password don't match...")]
        public string ConfirmPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
