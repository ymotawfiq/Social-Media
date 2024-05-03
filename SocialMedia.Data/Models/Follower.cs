
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Follower
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FollowerId { get; set; } = null!;
        public SiteUser? User { get; set; } = null;
    }
}
