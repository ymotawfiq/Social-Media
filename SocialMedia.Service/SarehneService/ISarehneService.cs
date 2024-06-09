

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.SarehneService
{
    public interface ISarehneService
    {
        Task<ApiResponse<SarehneMessage>> SendMessageAsync(
            SendSarahaMessageDto sendSarahaMessageDto, SiteUser user);
        Task<ApiResponse<SarehneMessage>> GetMessageAsync(string messageId, SiteUser user);
        Task<ApiResponse<SarehneMessage>> DeleteMessageAsync(string messageId, SiteUser user);
        Task<ApiResponse<IEnumerable<SarehneMessage>>> GetMessagesAsync(SiteUser user);
    }
}
