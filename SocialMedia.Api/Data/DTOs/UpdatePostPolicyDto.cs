

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdatePostPolicyDto
    {
        public string PostId { get; set; } = null!;
        public string PolicyIdOrName { get; set; } = null!;
    }
}
