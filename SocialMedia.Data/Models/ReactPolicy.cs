

namespace SocialMedia.Data.Models
{
    public class ReactPolicy
    {
        public string Id { get; set; } = null!;
        public string PolicyId { get; set; } = null!;
        public Policy? Policy { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
