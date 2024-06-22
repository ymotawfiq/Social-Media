using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class ChatMember
    {
        public string Id { get; set; } = null!;
        public string ChatId { get; set; } = null!;
        public string Member1Id { get; set; } = null!;
        public string? Member2Id { get; set; }
        public bool IsMember { get; set; }
        public Chat? Chat { get; set; }
        public SiteUser? User1 { get; set; }
        public SiteUser? User2 { get; set; }
        public List<ChatMemberRole>? ChatMemberRoles { get; set; }
    }
}
