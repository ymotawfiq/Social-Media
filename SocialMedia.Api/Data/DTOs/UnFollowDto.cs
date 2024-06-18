
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UnFollowDto
    {
        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
