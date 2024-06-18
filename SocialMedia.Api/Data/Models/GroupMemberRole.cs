
namespace SocialMedia.Api.Data.Models
{
    public class GroupMemberRole
    {
        public string Id { get; set; } = null!;
        public string GroupMemberId { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public GroupMember? GroupMember { get; set; }
        public GroupRole? GroupRole { get; set; }
    }
}
