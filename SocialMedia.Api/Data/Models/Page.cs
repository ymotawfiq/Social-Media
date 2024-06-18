

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class Page
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string CreatorId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public SiteUser? Creator { get; set; }
        public List<PagePost>? PagePosts { get; set; }
        public List<PageFollower>? PageFollowers { get; set; }
    }
}
