using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models;


namespace SocialMedia.Service.GroupAccessRequestService
{
    public interface IGroupAccessRequestService
    {
        Task<ApiResponse<object>> AddGroupAccessRequestAsync(
                AddGroupAccessRequestDto addGroupAccessRequestDto, SiteUser user);
        Task<ApiResponse<GroupAccessRequest>> DeleteGroupAccessRequestAsync(
            string groupAccessRequestId, SiteUser user);
    }
}
