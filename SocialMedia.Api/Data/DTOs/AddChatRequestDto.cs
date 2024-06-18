

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddChatRequestDto
    {
        [Required]
        public string UserIdOrNameOrEmail { get; set; } = null!;
    }
}
