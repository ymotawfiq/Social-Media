
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class PostCommentReplay
    {
        public string Id { get; set; } = null!;
        public string PostCommentId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? Replay { get; set; } = null!;
        public string? ReplayImage { get; set; } = null!;
        public string? PostCommentReplayId { get; set; }
        public PostCommentReplay? PostCommentReplayChildReplay { get; set; }
        public List<PostCommentReplay>? PostCommentReplays { get; set; }
        public PostComment? PostComment { get; set; }
        public SiteUser? User { get; set; }
    }
}
