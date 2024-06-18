

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.DTOs.Authentication.EmailConfirmation
{
    public class EmailConfirmationDto
    {
        public SiteUser User { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
