

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class FriendListPolicy
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PolicyId { get; set; } = null!;
        public Policy? Policy { get; set; }
        public SiteUser? User { get; set; } 
    }
}
