

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateSarehneMessagePolicyToAnotherDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
