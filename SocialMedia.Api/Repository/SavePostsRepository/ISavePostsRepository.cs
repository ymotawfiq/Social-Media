

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.SavePostsRepository
{
    public interface ISavePostsRepository : ICrud<SavedPosts>
    {
        Task<SavedPosts> UnSavePostAsync(SiteUser user, string postId);
        Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId, string folderId);
        Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId);
    }
}
