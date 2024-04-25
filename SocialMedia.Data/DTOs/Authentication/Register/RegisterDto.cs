
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs.Authentication.Register
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Please enter your first name")]
        [Length(3, 15, ErrorMessage ="First name length must be between 3 and 15 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your last name")]
        [Length(3, 15, ErrorMessage = "Last name length must be between 3 and 15 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress]
        [RegularExpression("^\\S+@\\S+\\.\\S+$")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter user name")]
        [Length(3, 20, ErrorMessage = "User name length must be between 3 and 20 characters")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your display name")]
        [Length(3, 20, ErrorMessage = "Display name length must be between 3 and 20 characters")]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter password for your account")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage ="Password and confirmation password don't match...")]
        public string ConfirmPassword { get; set; } = null!;

        public List<string> Roles { get; set; } = null!;
    }
}
