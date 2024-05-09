

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateUserPolicyDto
    {
        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;

        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
