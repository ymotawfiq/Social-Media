
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddChatMessageDto
    {
        [Required]
        public string ChatId { get; set; } = null!;
        public string? Message { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
