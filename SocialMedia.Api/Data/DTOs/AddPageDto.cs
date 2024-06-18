

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddPageDto
    {
        [Required]
        [Length(3, 50)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
