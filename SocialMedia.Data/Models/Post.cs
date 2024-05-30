

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class Post
    {
        public string Id { get; set; } = string.Empty;
        public string PolicyId { get; set; } = string.Empty;
        public string ReactPolicyId { get; set; } = string.Empty;
        public string CommentPolicyId { get; set; } = string.Empty;
        public string Content { get; set; } = null!;
        public DateTime PostedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Policy? Policy { get; set; }
        public ReactPolicy? ReactPolicy { get; set; }
        public CommentPolicy? CommentPolicy { get; set; }
        public List<UserPosts>? UserPosts { get; set; }
        public List<PostImages>? PostImages { get; set; }
        public List<PostView>? PostViews { get; set; }
        public List<SavedPosts>? SavedPosts { get; set; }
        public List<PostReacts>? PostReacts { get; set; }
        public List<PostComments>? PostComments { get; set; }
    }
}
