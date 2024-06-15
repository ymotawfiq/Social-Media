
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.DTOs;

namespace SocialMedia.Service.PageService
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
