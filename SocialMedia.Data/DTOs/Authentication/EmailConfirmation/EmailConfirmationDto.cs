

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.DTOs.Authentication.EmailConfirmation
{
    public class EmailConfirmationDto
    {
        public SiteUser User { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
