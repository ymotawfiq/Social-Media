

using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class PostView
    {
        public string Id { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public int ViewNumber { get; set; }
        public Post? Post { get; set; }
    }
}
