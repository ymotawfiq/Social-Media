

namespace SocialMedia.Data.Models
{
    public class CommentPolicy
    {
        public string Id { get; set; } = null!;
        public string PolicyId { get; set; } = null!;
        public Policy? Policy { get; set; }
    }
}
