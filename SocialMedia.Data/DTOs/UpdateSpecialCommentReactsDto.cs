

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateSpecialCommentReactsDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string ReactId { get; set; } = null!;
    }
}
