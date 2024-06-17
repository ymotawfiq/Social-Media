

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class MessageReact
    {
        public string Id { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public string ReactId { get; set; } = null!;
        public string ReactedUserId { get; set; } = null!;
        public ChatMessage? ChatMessage { get; set; }
        public React? React { get; set; }
        public SiteUser? User { get; set; }
    }
}
