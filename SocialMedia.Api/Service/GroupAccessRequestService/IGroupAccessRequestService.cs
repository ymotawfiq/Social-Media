using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models;


namespace SocialMedia.Api.Service.GroupAccessRequestService
{
    public interface IGroupAccessRequestService
    {
        Task<ApiResponse<object>> AddGroupAccessRequestAsync(
                AddGroupAccessRequestDto addGroupAccessRequestDto, SiteUser user);
        Task<ApiResponse<GroupAccessRequest>> DeleteGroupAccessRequestAsync(
            string groupAccessRequestId, SiteUser user);
    }
}
