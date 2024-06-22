using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.ChatService
{
    public interface IChatService
    {
        Task<ApiResponse<Chat>> AddPrivateChatAsync(AddChatDto addChatDto, SiteUser user);
        Task<ApiResponse<Chat>> AddNonPrivateChatAsync(AddChatDto addChatDto, SiteUser user);
        Task<ApiResponse<Chat>> GetChatAsync(string chatId);
    }
}
