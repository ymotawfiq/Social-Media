

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Block
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string BlockedUserId { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}
