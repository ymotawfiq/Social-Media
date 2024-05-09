

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddAccountPolicyDto
    {
        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
