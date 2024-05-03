

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class FollowerDto
    {
        public string? Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string FollowerId { get; set; } = null!;
    }
}
