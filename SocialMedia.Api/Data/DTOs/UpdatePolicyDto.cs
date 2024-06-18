
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdatePolicyDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string PolicyType { get; set; } = null!;
    }
}
