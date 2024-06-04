

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class FollowDto
    {
        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
