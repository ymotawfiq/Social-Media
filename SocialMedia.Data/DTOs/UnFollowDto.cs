
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UnFollowDto
    {
        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
