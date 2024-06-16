

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class ChatRequest
    {
        public string Id { get; set; } = null!;
        public string UserWhoSentRequestId { get; set; } = null!;
        public string UserWhoReceivedRequestId { get; set; } = null!;
        public SiteUser? UserWhoReceived { get; set; }
        public SiteUser? UserWhoSent { get; set; }
    }
}
