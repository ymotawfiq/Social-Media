
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateReactDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string ReactValue { get; set; } = null!;
    }
}
