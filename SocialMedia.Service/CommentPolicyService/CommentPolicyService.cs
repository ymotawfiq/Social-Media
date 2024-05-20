
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.CommentPolicyService
{
    public class CommentPolicyService : ICommentPolicyService
    {
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        private readonly IPolicyRepository _policyRepository;
        public CommentPolicyService(ICommentPolicyRepository _commentPolicyRepository,
            IPolicyRepository _policyRepository)
        {
            this._commentPolicyRepository = _commentPolicyRepository;
            this._policyRepository = _policyRepository;
        }
        public async Task<ApiResponse<CommentPolicy>> AddCommentPolicyAsync(CommentPolicyDto commentPolicyDto)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(commentPolicyDto.PolicyId);
            if (policy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Policy not found");
            }
            var reactPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(commentPolicyDto.PolicyId);
            if (reactPolicy != null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._400_BadRequest("Comment policy already exists");
            }
            var newComentPolicy = await _commentPolicyRepository.AddCommentPolicyAsync(
                ConvertFromDto.ConvertFromCommentPolicyDto_Add(commentPolicyDto));
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy added successfully", newComentPolicy);
        }

        public async Task<ApiResponse<CommentPolicy>> DeleteCommentPolicyByIdAsync(string commentPolicyId)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(commentPolicyId);
            if (commentPolicy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Comment policy not found");
            }
            var deletedCommentPolicy = await _commentPolicyRepository.DeleteCommentPolicyByIdAsync(commentPolicyId);
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy deleted successfully", deletedCommentPolicy);
        }

        public async Task<ApiResponse<CommentPolicy>> GetCommentPolicyByIdAsync(string commentPolicyId)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(commentPolicyId);
            if (commentPolicy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Comment policy not found");
            }
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy found successfully", commentPolicy);
        }

        public async Task<ApiResponse<IEnumerable<CommentPolicy>>> GetCommentPoliciesAsync()
        {
            var commentPolicies = await _commentPolicyRepository.GetCommentPoliciesAsync();
            if (commentPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<CommentPolicy>>
                    ._200_Success("No comment policies found", commentPolicies);
                    
            }
            return StatusCodeReturn<IEnumerable<CommentPolicy>>
                    ._200_Success("Comment policies found successfully", commentPolicies);
        }

        public async Task<ApiResponse<CommentPolicy>> UpdateCommentPolicyAsync(CommentPolicyDto commentPolicyDto)
        {
            if (commentPolicyDto.Id == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._400_BadRequest("Comment policy id must not be null");
            }
            var policy = await _policyRepository.GetPolicyByIdAsync(commentPolicyDto.PolicyId);
            if (policy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Policy not found");
            }
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(commentPolicyDto.PolicyId);
            if (commentPolicy == null)
            {
                return StatusCodeReturn<CommentPolicy>
                    ._404_NotFound("Comment policy not found");
            }
            var updatedComentPolicy = await _commentPolicyRepository.UpdateCommentPolicyAsync(
                ConvertFromDto.ConvertFromCommentPolicyDto_Update(commentPolicyDto));
            return StatusCodeReturn<CommentPolicy>
                ._200_Success("Comment policy updated successfully", updatedComentPolicy);
        }
    }
}
