

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdatePostCommentDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string PostId { get; set; } = null!;

        [Required]
        [Length(1, 500)]
        public string Comment { get; set; } = null!;
        public IFormFile? CommentImage { get; set; } = null!;
    }
}
