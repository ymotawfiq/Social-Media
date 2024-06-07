

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddGroupRoleDto
    {
        [Required]
        public string RoleName { get; set; } = null!;
    }
}
