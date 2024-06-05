

namespace SocialMedia.Data.Models
{
    public class PagePosts
    {
        public string Id { get; set; } = null!;
        public string PageId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public Page? Page { get; set; }
        public Post? Post { get; set; }
    }
}
