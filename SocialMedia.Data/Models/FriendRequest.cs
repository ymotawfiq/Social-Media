

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class FriendRequest
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string PersonId { get; set; } = null!;
        public bool IsAccepted { get; set; }
        public SiteUser? User { get; set; }

    }
}
