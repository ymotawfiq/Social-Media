
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs.Authentication.ResetEmail
{
    public class ResetEmailDto
    {
        [Required(ErrorMessage ="Please enter your old email")]
        public string OldEmail { get; set; } = null!;

        [Required(ErrorMessage = "Please enter new email")]
        public string NewEmail { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;
    }
}
