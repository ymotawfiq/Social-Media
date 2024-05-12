

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class PostView
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public int ViewNumber { get; set; }
        public SiteUser? User { get; set; }
        public Post? Post { get; set; }
    }
}
