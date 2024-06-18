

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Service.ChatRequestService
{
    public interface IChatRequestService
    {
        Task<ApiResponse<ChatRequest>> AddChatRequestAsync(AddChatRequestDto addChatRequestDto, SiteUser user);
        Task<ApiResponse<ChatRequest>> AcceptChatRequestAsync(string requestId, SiteUser user);
        Task<ApiResponse<ChatRequest>> DeleteChatRequestByIdAsync(string id, SiteUser user);
        Task<ApiResponse<ChatRequest>> GetChatRequestByIdAsync(string id, SiteUser user);
        Task<ApiResponse<ChatRequest>> GetChatRequestByUserAsync(SiteUser user1, SiteUser user2);
        Task<ApiResponse<IEnumerable<ChatRequest>>> GetReceivedChatRequestsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<ChatRequest>>> GetSentChatRequestsAsync(SiteUser user);
        
    }
}
