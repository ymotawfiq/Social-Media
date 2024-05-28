

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddUserSavedPostsFolderDto
    {
        [Required]
        [Length(3, 30)]
        public string FolderName { get; set; } = null!;
    }
}
