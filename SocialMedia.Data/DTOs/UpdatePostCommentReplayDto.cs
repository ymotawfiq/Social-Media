using Microsoft.AspNetCore.Http;


namespace SocialMedia.Data.DTOs
{
    public class UpdatePostCommentReplayDto
    {
        public string Id { get; set; } = null!;
        public string PostCommentId { get; set; } = null!;
        public string? Replay { get; set; } = null!;
        public IFormFile? ReplayImage { get; set; } = null!;
    }
}
