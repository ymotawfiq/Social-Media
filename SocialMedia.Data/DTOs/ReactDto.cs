

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class ReactDto
    {
        public string? Id { get; set; }

        [Required]
        public string ReactValue { get; set; } = null!;
    }
}
