

namespace SocialMedia.Data.Models
{
    public class PostImages
    {
        public string Id { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public Post? Post { get; set; }
    }
}
