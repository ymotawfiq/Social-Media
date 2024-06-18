

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class Policy
    {
        public string Id { get; set; } = null!;
        public string PolicyType { get; set; } = null!;
        public List<Post>? PostPolicies { get; set; }
        public List<Post>? ReactPolicies { get; set; }
        public List<Post>? CommentPolicies { get; set; }
        public List<SiteUser>? UserAccountPolicies { get; set; } 
        public List<SiteUser>? UserReactPolicies { get; set; } 
        public List<SiteUser>? UserCommentPolicies { get; set; } 
        public List<SiteUser>? UserFriendListPolicies { get; set; } 
        public List<SiteUser>? UserPostPolicies { get; set; } 
        public List<Group>? GroupPolicies { get; set; }
        public List<SarehneMessage>? SarehneMessagePolicies { get; set; } 
    }
}
