

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.DTOs.Authentication.User
{
    public class CreateUserResponse
    {
        public string Token { get; set; } = null!;
        public SiteUser User { get; set; } = null!;
    }
}
