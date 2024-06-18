

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateChatMessageDto
    {
        [Required]
        public string ChatId { get; set; } = null!;

        [Required]
        public string MessageId { get; set; } = null!;

        [Required]
        public string Message { get; set; } = null!;
    }
}
