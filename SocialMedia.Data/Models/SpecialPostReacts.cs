

namespace SocialMedia.Data.Models
{
    public class SpecialPostReacts
    {
        public string Id { get; set; } = null!;
        public string ReactId { get; set; } = null!;
        public React? React { get; set; }
        public List<PostReacts>? PostReacts { get; set; }
    }
}
