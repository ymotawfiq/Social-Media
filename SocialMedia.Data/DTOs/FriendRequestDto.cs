

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class FriendRequestDto
    {
        public string? Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string PersonId { get; set; } = null!;

        public bool IsAccepted { get; set; }
    }
}
