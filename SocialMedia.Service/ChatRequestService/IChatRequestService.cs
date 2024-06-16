

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models;

namespace SocialMedia.Service.ChatRequestService
{
    public interface IChatRequestService
    {
        Task<ApiResponse<ChatRequest>> AddChatRequestAsync(AddChatRequestDto addChatRequestDto, SiteUser user);
        Task<ApiResponse<ChatRequest>> DeleteChatRequestByIdAsync(string id, SiteUser user);
        Task<ApiResponse<ChatRequest>> GetChatRequestByIdAsync(string id, SiteUser user);
        Task<ApiResponse<ChatRequest>> GetChatRequestByUserAsync(SiteUser user1, SiteUser user2);
        Task<ApiResponse<IEnumerable<ChatRequest>>> GetReceivedChatRequestsAsync(SiteUser user);
        Task<ApiResponse<IEnumerable<ChatRequest>>> GetSentChatRequestsAsync(SiteUser user);
        
    }
}
