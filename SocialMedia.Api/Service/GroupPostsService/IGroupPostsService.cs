
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.GroupPostsService
{
    public interface IGroupPostsService
    {
        Task<object> AddGroupPostAsync(AddGroupPostDto addGroupPostDto, SiteUser user);
        Task<ApiResponse<GroupPost>> DeleteGroupPostByIdAsync(string groupPostId, SiteUser user);
        Task<ApiResponse<GroupPost>> GetGroupPostByIdAsync(string groupPostId);
        Task<ApiResponse<GroupPost>> GetGroupPostByIdAsync(string groupPostId, SiteUser user);
        Task<ApiResponse<GroupPost>> GetGroupPostByPostIdAsync(string postId, SiteUser user);
        Task<ApiResponse<IEnumerable<GroupPost>>> GetGroupPostsAsync(string groupId, SiteUser user);
        Task<ApiResponse<IEnumerable<GroupPost>>> GetGroupPostsAsync(string groupId);
    }
}
