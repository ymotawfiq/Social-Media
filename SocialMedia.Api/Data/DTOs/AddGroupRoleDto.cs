

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddGroupRoleDto
    {
        [Required]
        public string RoleName { get; set; } = null!;
    }
}
