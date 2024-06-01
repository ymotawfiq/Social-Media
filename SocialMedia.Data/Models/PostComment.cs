
using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class PostComment
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public string? CommentImage { get; set; } = null!;
        public SiteUser? User { get; set; }
        public Post? Post { get; set; }
        public List<PostCommentReplay>? PostCommentReplays { get; set; }

    }
}
