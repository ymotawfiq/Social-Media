

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateGroupRoleDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string RoleName { get; set; } = null!;
    }
}
