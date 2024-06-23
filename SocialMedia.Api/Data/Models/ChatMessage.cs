using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class ChatMessage
    {
        public string Id { get; set; } = null!;
        public string ChatId { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public string? Message { get; set; }
        public string? Photo { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Chat? Chat { get; set; }
        public SiteUser? User { get; set; }
        public List<MessageReact>? MessageReacts { get; set; }
    }
}
