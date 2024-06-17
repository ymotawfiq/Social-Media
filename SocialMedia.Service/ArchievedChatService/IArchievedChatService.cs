

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models;

namespace SocialMedia.Service.ArchievedChatService
{
    public interface IArchievedChatService
    {
        Task<ApiResponse<ArchievedChat>> ArchieveChatAsync(ArchieveChatDto archieveChatDto, SiteUser user);
        Task<ApiResponse<ArchievedChat>> UnArchieveChatByChatIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<ArchievedChat>> UnArchieveChatByIdAsync(string archievedChatId, SiteUser user);
        Task<ApiResponse<ArchievedChat>> GetArchieveChatByChatIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<ArchievedChat>> GetArchieveChatByIdAsync(string archievedChatId, SiteUser user);
        Task<ApiResponse<IEnumerable<ArchievedChat>>> GetUserArchieveChatsAsync(SiteUser user);
    }
}
