

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class GroupMember
    {
        public string Id { get; set; } = null!;
        public string GroupId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public Group? Group { get; set; }
        public SiteUser? User { get; set; }
        public List<GroupMemberRole>? GroupMemberRoles { get; set; }
    }
}
