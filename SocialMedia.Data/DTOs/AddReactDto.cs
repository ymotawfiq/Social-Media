

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddReactDto
    {
        [Required]
        public string ReactValue { get; set; } = null!;
    }
}
