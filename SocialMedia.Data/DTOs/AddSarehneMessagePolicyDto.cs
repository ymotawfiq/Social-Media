

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddSarehneMessagePolicyDto
    {
        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
