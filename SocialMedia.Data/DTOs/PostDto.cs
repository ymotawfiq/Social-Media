

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.DTOs
{
    public class PostDto
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

        public List<PostImages>? Images { get; set; }

    }
}
