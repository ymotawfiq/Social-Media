using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class MessageReact
    {
        public string Id { get; set; } = null!;
        public string MessageId { get; set; } = null!;
        public string ReactedUserId { get; set; } = null!;
        public string ReactId { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ChatMessage? Message { get; set; }
        public React? React { get; set; }
        public SiteUser? User { get; set; }
    }
}
