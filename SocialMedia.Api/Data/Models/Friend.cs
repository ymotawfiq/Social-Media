
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class Friend
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FriendId { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}
