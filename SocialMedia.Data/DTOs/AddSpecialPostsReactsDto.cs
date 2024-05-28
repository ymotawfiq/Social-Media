
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddSpecialPostsReactsDto
    {
        [Required]
        public string ReactId { get; set; } = null!;
    }
}
