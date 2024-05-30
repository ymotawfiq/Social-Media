

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class PostReacts
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string ReactId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public React? React { get; set; }
        public Post? Post { get; set; }
        public SiteUser? User { get; set; }
    }
}
