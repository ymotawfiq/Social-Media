
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddPagePostDto
    {
        [Required]
        public string PageId { get; set; } = null!;
        
        [Required]
        public string PostContent { get; set; } = string.Empty;

        public List<IFormFile>? Images { get; set; }
    }
}
