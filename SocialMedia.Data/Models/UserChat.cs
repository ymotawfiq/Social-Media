

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class UserChat
    {
        public string Id { get; set; } = null!;
        public string User1Id { get; set; } = null!;
        public string User2Id { get; set; } = null!;
        public SiteUser? User1 { get; set; }
        public SiteUser? User2 { get; set; }
    }
}
