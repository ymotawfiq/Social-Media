

namespace SocialMedia.Data.Models
{
    public class Policy
    {
        public string Id { get; set; } = null!;
        public string PolicyType { get; set; } = null!;
        public List<ReactPolicy>? ReactPolicies { get; set; }
        public List<CommentPolicy>? CommentPolicies { get; set; }
        public List<Post>? Posts { get; set; }
        public List<AccountPolicy>? AccountPolicies { get; set; } 
        public List<FriendListPolicy>? FriendListPolicies { get; set; }
        public List<AccountPostsPolicy>? PostPolicies { get; set; }
        public List<GroupPolicy>? GroupPolicies { get; set; }
    }
}
