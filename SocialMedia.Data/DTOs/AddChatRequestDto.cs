

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddChatRequestDto
    {
        [Required]
        public string UserIdOrNameOrEmail { get; set; } = null!;
    }
}
