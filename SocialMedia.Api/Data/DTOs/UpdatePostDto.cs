

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdatePostDto
    {
        [Required]
        public string PostId { get; set; } = null!;

        [Required]
        public string PostContent { get; set; } = string.Empty;

        public List<IFormFile>? Images { get; set; }
    }
}
