
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.PostCommentReplayService
{
    public interface IPostCommentReplayService
    {
        Task<ApiResponse<PostCommentReplay>> AddCommentReplayAsync(
            AddPostCommentReplayDto addPostCommentReplayDto, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> UpdateCommentReplayAsync(
            UpdatePostCommentReplayDto updatePostCommentReplayDto, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> DeleteCommentReplayByIdAsync(
            string commentReplayById, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> GetCommentReplayByIdAsync(
            string commentReplayById, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> AddReplayToReplayAsync(
            AddReplayToReplayCommentDto addReplayToReplayCommentDto, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> UpdateReplayToReplayAsync(
            UpdateReplayToReplayCommentDto updateReplayToReplayCommentDto, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> DeleteReplayToReplayByIdAsync(
            string commentReplayToReplayById, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> DeleteReplayImageAsync(
            string commentReplayToReplayById, SiteUser user);
        Task<ApiResponse<PostCommentReplay>> GetReplayToReplayByIdAsync(
            string commentReplayToReplayById, SiteUser user);
        Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetCommentReplaysByCommentIdAsync(
            string commentId, SiteUser user);
        Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetCommentReplaysByCommentIdAsync(string commentId);
        Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetReplaysOfReplayAsync(string replayId);
        Task<ApiResponse<IEnumerable<PostCommentReplay>>> GetReplaysOfReplayAsync(
            string replayId, SiteUser user);

    }
}
