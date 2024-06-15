

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddCommentReplayDto
    {
        [Required]
        public string PostId { get; set; } = null!;

        [Required]
        public string CommentId { get; set; } = null!;

        [Required]
        [Length(1, 500)]
        public string Comment { get; set; } = null!;
        public IFormFile? CommentImage { get; set; } = null!;
    }
}
