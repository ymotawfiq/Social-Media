

namespace SocialMedia.Data.Models
{
    public class SarehneMessagePolicy
    {
        public string Id { get; set; } = null!;
        public string PolicyId { get; set; } = null!;
        public Policy? Policy { get; set; }
        public List<SarehneMessage>? SarehneMessages { get; set; }
    }
}
