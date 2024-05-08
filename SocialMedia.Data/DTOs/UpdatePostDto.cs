

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdatePostDto
    {
        [Required]
        public string PostId { get; set; } = null!;

        [Required]
        public string PolicyId { get; set; } = string.Empty;

        [Required]
        public string ReactPolicyId { get; set; } = string.Empty;

        [Required]
        public string CommentPolicyId { get; set; } = string.Empty;

        [Required]
        public string PostContent { get; set; } = string.Empty;

        public List<IFormFile>? Images { get; set; }
    }
}
