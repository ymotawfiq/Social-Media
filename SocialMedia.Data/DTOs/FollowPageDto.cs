
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class FollowPageDto
    {
        [Required]
        public string PageId { get; set; } = null!;
    }
}
