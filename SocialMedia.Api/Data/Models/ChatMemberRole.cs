namespace SocialMedia.Api.Data.Models
{
    public class ChatMemberRole
    {
        public string Id { get; set; } = null!;
        public string ChatMemberId { get; set; } = null!;
        public string RoleId { get; set; } = null!;
        public ChatMember? ChatMember { get; set; }
        public Role? Role { get; set; }
    }
}
