

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateFriendRequestDto
    {
        [Required]
        public string FriendRequestId { get; set; } = null!;
        public bool IsAccepted { get; set; }
    }
}
