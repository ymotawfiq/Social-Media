

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class GroupAccessRequest
    {
        public string Id { get; set; } = null!;
        public string GroupId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public Group? Group { get; set; }
        public SiteUser? User { get; set; }
    }
}
