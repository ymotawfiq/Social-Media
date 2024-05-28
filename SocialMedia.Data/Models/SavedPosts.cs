

using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class SavedPosts
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostId { get; set; } = null!;
        public string FolderId { get; set; } = null!;
        public SiteUser? User { get; set; }
        public Post? Post { get; set; }
        public UserSavedPostsFolders? UserSavedPostsFolder { get; set; }
    }
}
