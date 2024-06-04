

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateReactPolicyDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string PolicyId { get; set; } = null!;
    }
}
