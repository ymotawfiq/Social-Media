

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.DTOs.Authentication.User
{
    public class LoginOtpResponse
    {
        public string Token { get; set; } = null!;
        public bool IsTwoFactorEnabled { get; set; }
        public SiteUser User { get; set; } = null!;
    }
}
