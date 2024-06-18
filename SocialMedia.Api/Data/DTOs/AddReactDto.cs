

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddReactDto
    {
        [Required]
        public string ReactValue { get; set; } = null!;
    }
}
