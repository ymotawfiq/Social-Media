

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class ChatMessage
    {
        public string Id { get; set; } = null!;
        public string ChatId { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public string? Message { get; set; }
        public string? Photo { get; set; }
        public string? MessageId { get; set; }
        public ChatMessage? MessageReplay { get; set; }
        public List<ChatMessage>? MessageReplays { get; set; }
        public DateTime SentAt { get; set; }
        public SiteUser? User { get; set; }
        public UserChat? Chat { get; set; }
        public List<MessageReact>? MessageReacts { get; set; }
    }
}
