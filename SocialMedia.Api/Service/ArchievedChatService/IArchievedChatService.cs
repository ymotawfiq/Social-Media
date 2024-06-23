using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.ArchievedChatService
{
    public interface IArchievedChatService
    {
        Task<ApiResponse<ArchievedChat>> ArchieveChatAsync(string chatId, SiteUser user);
        Task<ApiResponse<ArchievedChat>> UnArchieveChatByChatIdAsync(string chatId, SiteUser user);
        Task<ApiResponse<ArchievedChat>> UnArchieveChatByArchievedChatIdAsync(string archievedChatId,
            SiteUser user);
        Task<ApiResponse<IEnumerable<ArchievedChat>>> GetUserArchieveChatsAsync(SiteUser user);
    }
}
