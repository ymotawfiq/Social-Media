

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class SarehneMessage
    {
        public string Id { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public string SenderName { get; set; } = null!;
        public string Message { get; set; } = null!;
        public SiteUser? User { get; set; }
    }
}
