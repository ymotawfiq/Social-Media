

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class FriendRequest
    {
        public Guid Id { get; set; }
        public string UserWhoSendId { get; set; } = null!;
        public string UserWhoReceivedId { get; set; } = null!;
        public bool IsAccepted { get; set; }
        public SiteUser? User { get; set; }

    }
}
