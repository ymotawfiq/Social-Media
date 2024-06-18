

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class FollowPageUserDto
    {
        [Required]
        public string PageId { get; set; } = null!;

        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
