
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.ChatMessageService
{
    public interface IChatMessageService
    {
        Task<ApiResponse<ChatMessage>> SendMessageAsync(AddChatMessageDto addChatMessageDto, SiteUser user);
        Task<ApiResponse<ChatMessage>> ReplayToMessageAsync(AddChatMessageReplayDto addChatMessageReplayDto,
            SiteUser user);
        Task<ApiResponse<ChatMessage>> DeleteMessageAsync(string messageId, string chatId, SiteUser user);
        Task<ApiResponse<ChatMessage>> GetMessageAsync(string messageId, string chatId, SiteUser user);
        Task<ApiResponse<ChatMessage>> UpdateMessageAsync(UpdateChatMessageDto updateChatMessageDto,
            SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMessage>>> GetUserMessagesAsync(SiteUser user, string chatId);
    }
}
