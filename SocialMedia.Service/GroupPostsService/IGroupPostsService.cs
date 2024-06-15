
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.GroupPostsService
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
