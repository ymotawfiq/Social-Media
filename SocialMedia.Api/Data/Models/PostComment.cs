
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class PostComment
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public string? CommentId { get; set; }
        public string? CommentImage { get; set; } = null!;
        public SiteUser? User { get; set; }
        public Post? Post { get; set; }
        public PostComment? BaseComment { get; set; }
        public List<PostComment>? Replays { get; set; }

    }
}
