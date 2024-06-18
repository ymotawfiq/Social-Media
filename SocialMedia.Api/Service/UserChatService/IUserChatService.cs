

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.UserChatService
{
    public interface IUserChatService
    {
        Task<ApiResponse<UserChat>> AddUserChatAsync(AddUserChatDto addUserChatDto, SiteUser user);
        Task<ApiResponse<UserChat>> DeleteUserChatByIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<UserChat>> DeleteUserChatByUserAsync(string user1Id, string user2Id);
        Task<ApiResponse<UserChat>> GetUserChatByIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<IEnumerable<UserChat>>> GetUserChatsAsync(SiteUser user);
    }
}
