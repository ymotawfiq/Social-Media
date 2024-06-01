

using Microsoft.AspNetCore.Http;

namespace SocialMedia.Data.DTOs
{
    public class AddReplayToReplayCommentDto
    {
        public string CommentReplayId { get; set; } = null!;
        public string? Replay { get; set; } = null!;
        public IFormFile? ReplayImage { get; set; } = null!;
    }
}
