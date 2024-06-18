
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddMessageReactDto
    {
        [Required]
        public string MessageId { get; set; } = null!;

        [Required]
        public string ReactId { get; set; } = null!;
    }
}
