

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class FriendRequest
    {
        public string Id { get; set; } = null!;
        public string UserWhoSendId { get; set; } = null!;
        public string UserWhoReceivedId { get; set; } = null!;
        public bool IsAccepted { get; set; }
        public SiteUser? User { get; set; }

    }
}
