

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddPolicyDto
    {
        [Required]
        public string PolicyType { get; set; } = null!;
    }
}
