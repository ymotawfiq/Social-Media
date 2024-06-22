

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class AddRoleDto
    {
        [Required]
        public string RoleName { get; set; } = null!;
    }
}
