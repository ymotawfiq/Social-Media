

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdatePostReactPolicyDto
    {
        public string PostId { get; set; } = null!;
        public string PolicyIdOrName { get; set; } = null!;
    }
}
