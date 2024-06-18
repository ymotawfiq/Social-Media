

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class PostReacts
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostReactId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public React? React { get; set; }
        public Post? Post { get; set; }
        public SiteUser? User { get; set; }
    }
}
