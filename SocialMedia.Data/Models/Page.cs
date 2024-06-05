

namespace SocialMedia.Data.Models
{
    public class Page
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<UserPage>? UserPages { get; set; }
        public List<PagePosts>? PagePosts { get; set; }
    }
}
