

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateSarehneMessagePolicyDto
    {
        [Required]
        public string MessageId { get; set; } = null!;

        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
