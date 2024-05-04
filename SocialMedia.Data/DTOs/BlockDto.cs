

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class BlockDto
    {
        public string? Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string BlockedUserId { get; set; } = null!;
    }
}
