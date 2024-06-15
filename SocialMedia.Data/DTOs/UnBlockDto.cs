

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UnBlockDto
    {

        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
