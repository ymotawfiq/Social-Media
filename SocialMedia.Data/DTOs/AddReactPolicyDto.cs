

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddReactPolicyDto
    {
        [Required]
        public string PolicyId { get; set; } = null!;
    }
}
