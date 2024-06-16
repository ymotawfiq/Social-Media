

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.UserChatService
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
