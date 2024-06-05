

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PagePostsService
{
    public interface IPagePostsService
    {
        Task<ApiResponse<object>> AddPagePostAsync(AddPagePostDto addPagePostDto, SiteUser user);
        Task<ApiResponse<object>> DeletePagePostByIdAsync(string pagePostId, SiteUser user);
        Task<ApiResponse<object>> DeletePagePostByPostIdAsync(string postId, SiteUser user);
        Task<ApiResponse<object>> GetPagePostByIdAsync(string pagePostId);
    }
}
