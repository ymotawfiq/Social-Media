using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddChatMemberDto
    {
        [Required]
        public string ChatId { get; set; } = null!;

        [Required]
        public string UserIdOrNameOrEmail { get; set; } = null!;
    }
}
