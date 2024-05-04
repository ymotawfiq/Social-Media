

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class BlockUserDto
    {
        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
