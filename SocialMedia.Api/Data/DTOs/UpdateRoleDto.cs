

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateRoleDto
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string RoleName { get; set; } = null!;
    }
}
