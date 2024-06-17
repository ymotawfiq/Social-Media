
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class ArchievedChat
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ChatId { get; set; } = null!;
        public SiteUser? User { get; set; }
        public UserChat? UserChat { get; set; }
    }
}
