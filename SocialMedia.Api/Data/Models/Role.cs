

namespace SocialMedia.Api.Data.Models
{
    public class Role
    {
        public string Id { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public List<GroupMemberRole>? GroupMemberRoles { get; set; }
        public List<ChatMemberRole>? ChatMemberRoles { get; set; }

    }
}
