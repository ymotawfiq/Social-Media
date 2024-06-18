

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class PageFollower
    {
        public string Id { get; set; } = null!;
        public string PageId { get; set; } = null!;
        public string FollowerId { get; set; } = null!;
        public Page? Page { get; set; }
        public SiteUser? User { get; set; }
    }
}
