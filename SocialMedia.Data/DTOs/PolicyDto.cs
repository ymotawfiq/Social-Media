

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class PolicyDto
    {
        public string? Id { get; set; }

        [Required]
        public string PolicyType { get; set; } = null!;
    }
}
