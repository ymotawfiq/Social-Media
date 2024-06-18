
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.GroupAccessRequestService
{
    public interface IGroupAccessRequestService
    {
        Task<ApiResponse<GroupAccessRequest>> AddGroupAccessRequestAsync(
            AddGroupAccessRequestDto addGroupAccessRequestDto, SiteUser user);
        Task<ApiResponse<GroupAccessRequest>> DeleteGroupAccessRequestAsync(
            string groupAccessRequestId, SiteUser user);
    }
}
