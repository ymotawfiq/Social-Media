
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class GroupPost
    {
        public string Id { get; set; } = null!;
        public string GroupId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public Group? Group { get; set; }
        public Post? Post { get; set; }
        public SiteUser? User { get; set; }
    }
}
