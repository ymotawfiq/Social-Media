

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateUserSavedPostsFolderDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [Range(3, 30)]
        public string FolderName { get; set; } = null!;
    }
}
