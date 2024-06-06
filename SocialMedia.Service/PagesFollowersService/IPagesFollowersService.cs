

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PagesFollowersService
{
    public interface IPagesFollowersService
    {
        Task<ApiResponse<object>> FollowPageAsync(FollowPageDto followPageDto, SiteUser user);
        Task<ApiResponse<object>> FollowPageAsync(FollowPageUserDto followPageUserDto);
        Task<ApiResponse<object>> UnFollowPageAsync(UnFollowPageDto unFollowPageDto, SiteUser user);
        Task<ApiResponse<object>> UnFollowPageAsync(string pageFollowersId, SiteUser user);
        Task<ApiResponse<object>> GetPageFollowerAsync(string pageId, SiteUser user);
        Task<ApiResponse<object>> GetPageFollowerAsync(string pageFollowersId);
        Task<ApiResponse<object>> GetPageFollowersByPageIdAsync(string pageId);
    }
}
