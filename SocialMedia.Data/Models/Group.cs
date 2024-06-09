

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Group
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CreatedUserId { get; set; } = null!;
        public string GroupPolicyId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public SiteUser? User { get; set; }
        public GroupPolicy? GroupPolicy { get; set; }
        public List<GroupMember>? GroupMembers { get; set; }
        public List<GroupAccessRequest>? GroupAccessRequests { get; set; }
        public List<GroupPost>? GroupPosts { get; set; }
    }
}
