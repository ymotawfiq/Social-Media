
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Friend
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FriendId { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}
