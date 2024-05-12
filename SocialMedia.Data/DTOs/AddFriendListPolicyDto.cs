

namespace SocialMedia.Data.DTOs
{
    public class AddFriendListPolicyDto
    {
        public string UserIdOrNameOrEmail { get; set; } = null!;
        public string PolicyIdOrName { get; set; } = null!;
    }
}
