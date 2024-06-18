

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UnFollowPageDto
    {
        [Required]
        public string PageId { get; set; } = null!;
    }
}
