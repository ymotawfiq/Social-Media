
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.DTOs;

namespace SocialMedia.Api.Service.PageService
{
    public interface IPageService
    {
        Task<ApiResponse<Page>> AddPageAsync(AddPageDto addPageDto, SiteUser user);
        Task<ApiResponse<Page>> UpdatePageAsync(UpdatePageDto updatePageDto, SiteUser user);
        Task<ApiResponse<Page>> GetPageByIdAsync(string pageId);
        Task<ApiResponse<Page>> DeletePageByIdAsync(string pageId, SiteUser user);
        Task<ApiResponse<IEnumerable<Page>>> GetPagesByUserIdAsync(string userId);
    }
}
