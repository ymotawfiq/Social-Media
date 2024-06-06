

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UnFollowPageDto
    {
        [Required]
        public string PageId { get; set; } = null!;
    }
}
