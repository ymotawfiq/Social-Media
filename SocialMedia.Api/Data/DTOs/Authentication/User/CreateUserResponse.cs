

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.DTOs.Authentication.User
{
    public class CreateUserResponse
    {
        public string Token { get; set; } = null!;
        public SiteUser User { get; set; } = null!;
    }
}
