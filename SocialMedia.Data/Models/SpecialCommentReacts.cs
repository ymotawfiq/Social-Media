

namespace SocialMedia.Data.Models
{
    public class SpecialCommentReacts
    {
        public string Id { get; set; } = null!;
        public string ReactId { get; set; } = null!;
        public React? React { get; set; }
    }
}
