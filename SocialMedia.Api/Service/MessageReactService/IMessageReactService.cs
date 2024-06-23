using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.MessageReactService
{
    public interface IMessageReactService
    {

        Task<ApiResponse<MessageReact>> ReactToMessageAsync(AddMessageReactDto addMessageReactDto,
            SiteUser user);

        Task<ApiResponse<MessageReact>> UnReactToMessageByMessageIdAsync(string messageId, SiteUser user);

        Task<ApiResponse<MessageReact>> UnReactToMessageByMessageReactIdAsync(string messageReactId,
            SiteUser user);

        Task<ApiResponse<IEnumerable<MessageReact>>> GetReactsByMessageIdAsync(string messageId,
            SiteUser user);

    }
}
