
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddUserChatDto
    {
        [Required]
        public string UserIdOrNameOrEmail { get; set; } = null!;
    }
}
