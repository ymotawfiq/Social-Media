

namespace SocialMedia.Data.Models
{
    public class React
    {
        public string Id { get; set; } = null!;
        public string ReactValue { get; set; } = null!;
        public List<SpecialPostReacts>? SpecialPostReacts { get; set; }
        public List<SpecialCommentReacts>? SpecialCommentReacts { get; set; }
        public List<PostReacts>? PostReacts { get; set; }
    }
}
