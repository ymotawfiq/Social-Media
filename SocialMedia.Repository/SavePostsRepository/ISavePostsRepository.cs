

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.SavePostsRepository
{
    public interface ISavePostsRepository : ICrud<SavedPosts>
    {
        Task<SavedPosts> UnSavePostAsync(SiteUser user, string postId);
        Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId, string folderId);
        Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId);
    }
}
