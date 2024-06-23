using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class PrivateChat
    {
        public string Id { get; set; } = null!;
        public string User1Id { get; set; } = null!;
        public string User2Id { get; set; } = null!;
        public bool IsBlockedByUser1 { get; set; }
        public bool IsBlockedByUser2 { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAccepted { get; set; }
        public string ChatId { get; set; } = null!;
        public Chat? Chat { get; set; }
        public SiteUser? User1 { get; set; }
        public SiteUser? User2 { get; set; }
    }
}
