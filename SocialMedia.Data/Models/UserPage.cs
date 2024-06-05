

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class UserPage
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PageId { get; set; } = null!;
        public SiteUser? User { get; set; }
        public Page? Page { get; set; }
    }
}
