
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Models
{
    public class UserSavedPostsFolders
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string FolderName { get; set; } = null!;
        public List<SavedPosts>? SavedPosts { get; set; }
        public SiteUser? User { get; set; }
    }
}
