
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddSpecialCommentReactsDto
    {
        [Required]
        public string ReactId { get; set; } = null!;
    }
}
