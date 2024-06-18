

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Data.DTOs
{
    public class UpdateExistGroupPolicyDto
    {
        [Required]
        public string GroupId { get; set; } = null!;

        [Required]
        public string GroupPolicyIdOrName { get; set; } = null!;
    }
}
