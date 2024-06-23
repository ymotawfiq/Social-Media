using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class ChatMember
    {
        public string Id { get; set; } = null!;
        public string ChatId { get; set; } = null!;
        public string MemberId { get; set; } = null!;
        public bool IsMember { get; set; }
        public Chat? Chat { get; set; }
        public SiteUser? User { get; set; }
        public List<ChatMemberRole>? ChatMemberRoles { get; set; }
    }
}
