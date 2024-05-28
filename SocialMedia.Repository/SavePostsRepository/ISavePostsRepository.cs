

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.SavePostsRepository
{
    public interface ISavePostsRepository
    {
        Task<SavedPosts> SavePostAsync(SavedPosts savedPosts);
        Task<SavedPosts> UnSavePostAsync(SiteUser user, string postId);
        Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId, string folderId);
        Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId);
        Task SaveChangesAsync();
    }
}
