
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddChatMessageReplayDto
    {
        [Required]
        public string MessageId { get; set; } = null!;
        public string? Message { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
