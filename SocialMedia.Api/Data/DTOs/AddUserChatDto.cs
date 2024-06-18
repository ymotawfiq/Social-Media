
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddUserChatDto
    {
        [Required]
        public string UserIdOrNameOrEmail { get; set; } = null!;
    }
}
