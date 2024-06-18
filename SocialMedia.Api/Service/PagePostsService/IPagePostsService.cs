

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.PagePostsService
{
    public interface IPagePostsService
    {
        Task<ApiResponse<object>> AddPagePostAsync(AddPagePostDto addPagePostDto, SiteUser user);
        Task<ApiResponse<object>> DeletePagePostByIdAsync(string pagePostId, SiteUser user);
        Task<ApiResponse<object>> DeletePagePostByPostIdAsync(string postId, SiteUser user);
        Task<ApiResponse<object>> GetPagePostByIdAsync(string pagePostId);
    }
}
