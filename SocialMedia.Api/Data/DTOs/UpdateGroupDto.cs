

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateGroupDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [Length(3, 50)]
        public string Name { get; set; } = null!;

        [Required]
        [Length(3, 1000)]
        public string Description { get; set; } = null!;

    }
}
