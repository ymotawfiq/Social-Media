

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.PagesFollowersService
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
