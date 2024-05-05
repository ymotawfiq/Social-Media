

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class CommentPolicyDto
    {
        public string? Id { get; set; } = null!;

        [Required]
        public string PolicyId { get; set; } = null!;
    }
}
