using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.ChatMessageService
{
    public interface IChatMessageService
    {
        Task<ApiResponse<ChatMessage>> SendMessageAsync(AddChatMessageDto addChatMessageDto,
            SiteUser user);
        Task<ApiResponse<ChatMessage>> UnSendMessageAsync(string messageId, SiteUser user);
        Task<ApiResponse<ChatMessage>> UnpdateMessageAsync(UpdateChatMessageDto updateChatMessageDto,
            SiteUser user);
        Task<ApiResponse<IEnumerable<ChatMessage>>> GetChatMessagesAsync(string chatId, SiteUser user);

    }
}
