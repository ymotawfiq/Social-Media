
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class ArchieveChatDto
    {
        [Required]
        public string ChatId { get; set; } = null!;
    }
}
