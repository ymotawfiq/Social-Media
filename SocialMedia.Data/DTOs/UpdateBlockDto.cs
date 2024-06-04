

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateBlockDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
