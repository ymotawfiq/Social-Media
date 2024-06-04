

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddBlockDto
    {
        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
