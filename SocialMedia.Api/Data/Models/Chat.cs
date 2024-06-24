using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Models
{
    public class Chat
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string PolicyId { get; set; } = null!;
        public string CreatorId { get; set; } = null!;
        public SiteUser? User { get; set; }
        public Policy? Policy { get; set; }
        public List<ChatMember>? ChatMembers { get; set; }
        public List<PrivateChat>? PrivateChats { get; set; }
        public List<ChatMessage>? ChatMessages { get; set; }
        public List<ArchievedChat>? ArchievedChats { get; set; }

    }
}
