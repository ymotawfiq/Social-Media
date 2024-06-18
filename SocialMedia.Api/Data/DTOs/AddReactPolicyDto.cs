

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddReactPolicyDto
    {
        [Required]
        public string PolicyId { get; set; } = null!;
    }
}
