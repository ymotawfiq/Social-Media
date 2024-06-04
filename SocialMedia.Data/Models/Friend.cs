
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Friend
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FriendId { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}
