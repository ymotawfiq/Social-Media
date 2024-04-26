

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs.Authentication.UpdateAccount
{
    public class UpdateAccountInfoDto
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [Length(3, 15, ErrorMessage = "First name length must be between 3 and 15 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your last name")]
        [Length(3, 15, ErrorMessage = "Last name length must be between 3 and 15 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your display name")]
        [Length(3, 20, ErrorMessage = "Display name length must be between 3 and 20 characters")]
        public string DisplayName { get; set; } = null!;
    }
}
