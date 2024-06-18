
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.SavedPostsService
{
    public interface ISavedPostsService
    {
        Task<ApiResponse<SavedPosts>> SavePostAsync(SiteUser user, SavePostDto savePostDto);
        Task<ApiResponse<SavedPosts>> UnSavePostAsync(SiteUser user, string postId);
    }
}
