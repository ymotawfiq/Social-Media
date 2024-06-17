
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class ArchieveChatDto
    {
        [Required]
        public string ChatId { get; set; } = null!;
    }
}
