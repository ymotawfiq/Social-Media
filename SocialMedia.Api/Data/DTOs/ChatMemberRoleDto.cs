using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class ChatMemberRoleDto
    {
        [Required]
        public string ChatMemberId { get; set; } = null!;


        [Required]
        public string RoleIdOrName { get; set; } = null!;
    }
}
