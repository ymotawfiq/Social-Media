

namespace SocialMedia.Data.Models
{
    public class Policy
    {
        public string Id { get; set; } = null!;
        public string PolicyType { get; set; } = null!;
        public List<ReactPolicy>? ReactPolicies { get; set; }
        public List<CommentPolicy>? CommentPolicies { get; set; }
    }
}
