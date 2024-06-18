

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UnBlockDto
    {

        [Required]
        public string UserIdOrUserNameOrEmail { get; set; } = null!;
    }
}
