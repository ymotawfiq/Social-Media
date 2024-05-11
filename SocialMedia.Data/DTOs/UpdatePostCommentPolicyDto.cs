
namespace SocialMedia.Data.DTOs
{
    public class UpdatePostCommentPolicyDto
    {
        public string PostId { get; set; } = null!;
        public string PolicyIdOrName { get; set; } = null!;
    }
}
