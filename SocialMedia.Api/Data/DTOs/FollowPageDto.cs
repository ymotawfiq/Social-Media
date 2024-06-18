
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class FollowPageDto
    {
        [Required]
        public string PageId { get; set; } = null!;
    }
}
