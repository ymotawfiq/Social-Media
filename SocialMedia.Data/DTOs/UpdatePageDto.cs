
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdatePageDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [Length(3, 50)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
