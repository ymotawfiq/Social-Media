

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.DTOs.Authentication.User
{
    public class LoginOtpResponse
    {
        public string Token { get; set; } = null!;
        public bool IsTwoFactorEnabled { get; set; }
        public SiteUser User { get; set; } = null!;
    }
}
