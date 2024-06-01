

using Microsoft.AspNetCore.Http;

namespace SocialMedia.Data.DTOs
{
    public class UpdateReplayToReplayCommentDto
    {
        public string ReplayId { get; set; } = null!;
        public string? Replay { get; set; } = null!;
        public IFormFile? ReplayImage { get; set; } = null!;
    }
}
