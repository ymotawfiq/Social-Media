
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class UserPosts
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public Post? Post { get; set; }
        public SiteUser? User { get; set; } = null!;
    }
}
