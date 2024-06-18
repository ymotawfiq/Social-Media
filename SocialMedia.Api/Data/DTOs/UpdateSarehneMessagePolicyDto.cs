

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateSarehneMessagePolicyDto
    {
        [Required]
        public string MessageId { get; set; } = null!;

        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
