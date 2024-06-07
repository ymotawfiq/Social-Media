

using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Data.DTOs
{
    public class AddGroupPolicyDto
    {

        [Required]
        public string PolicyIdOrName { get; set; } = null!;
    }
}
