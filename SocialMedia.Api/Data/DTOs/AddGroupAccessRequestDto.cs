

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddGroupAccessRequestDto
    {
        [Required]
        public string GroupId { get; set; } = null!;

    }
}
