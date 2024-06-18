

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddFriendDto
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string FriendId { get; set; } = null!;
    }
}
