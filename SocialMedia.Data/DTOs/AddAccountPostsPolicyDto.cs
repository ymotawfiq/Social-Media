

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddAccountPostsPolicyDto
    {
        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
