

namespace SocialMedia.Api.Data.DTOs.Authentication.UpdateAccount
{
    public class UpdateAccountRolesDto
    {
        public string UserNameOrEmail { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
    }
}
