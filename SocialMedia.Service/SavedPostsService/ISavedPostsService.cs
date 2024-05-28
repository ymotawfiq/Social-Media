
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.SavedPostsService
{
    public interface ISavedPostsService
    {
        Task<ApiResponse<SavedPosts>> SavePostAsync(SiteUser user, SavePostDto savePostDto);
        Task<ApiResponse<SavedPosts>> UnSavePostAsync(SiteUser user, string postId);
    }
}
