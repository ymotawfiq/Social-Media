
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdatePostReactDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string ReactId { get; set; } = null!;

        [Required]
        public string PostId { get; set; } = null!;
    }
}
