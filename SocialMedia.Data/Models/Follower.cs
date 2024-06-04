
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Follower
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FollowerId { get; set; } = null!;
        public SiteUser? User { get; set; } = null;
    }
}
