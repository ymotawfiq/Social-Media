

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddCommentPolicyDto
    {
        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
