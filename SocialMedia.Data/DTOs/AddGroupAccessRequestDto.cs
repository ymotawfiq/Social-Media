

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddGroupAccessRequestDto
    {
        [Required]
        public string GroupId { get; set; } = null!;

    }
}
