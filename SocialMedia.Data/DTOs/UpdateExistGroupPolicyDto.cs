

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class UpdateExistGroupPolicyDto
    {
        [Required]
        public string GroupId { get; set; } = null!;

        [Required]
        public string GroupPolicyIdOrName { get; set; } = null!;
    }
}
