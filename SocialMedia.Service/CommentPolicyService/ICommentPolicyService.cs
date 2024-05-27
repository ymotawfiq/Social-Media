
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models;

namespace SocialMedia.Service.CommentPolicyService
{
    public interface ICommentPolicyService
    {
        Task<ApiResponse<CommentPolicy>> AddCommentPolicyAsync(CommentPolicyDto commentPolicyDto);
        Task<ApiResponse<CommentPolicy>> UpdateCommentPolicyAsync(CommentPolicyDto commentPolicyDto);
        Task<ApiResponse<CommentPolicy>> DeleteCommentPolicyByIdAsync(string commentPolicyId);
        Task<ApiResponse<CommentPolicy>> GetCommentPolicyByIdAsync(string commentPolicyId);
        Task<ApiResponse<CommentPolicy>> GetCommentPolicyAsync(string commentPolicyIdOrPolicyName);
        Task<ApiResponse<IEnumerable<CommentPolicy>>> GetCommentPoliciesAsync();
    }
}
