

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class ReactPolicyDto
    {
        public string? Id { get; set; }

        [Required]
        public string PolicyId { get; set; } = null!;
    }
}
