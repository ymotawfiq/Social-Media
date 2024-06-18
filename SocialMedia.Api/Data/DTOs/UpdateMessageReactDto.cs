

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateMessageReactDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string ReactId { get; set; } = null!;
    }
}
