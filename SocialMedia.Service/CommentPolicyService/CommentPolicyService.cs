
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.PolicyRepository;

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
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            var reactPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(commentPolicyDto.PolicyId);
            if (reactPolicy != null)
            {
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Comment policy already exists",
                    StatusCode = 400
                };
            }
            var newComentPolicy = await _commentPolicyRepository.AddCommentPolicyAsync(
                ConvertFromDto.ConvertFromCommentPolicyDto_Add(commentPolicyDto));
            return new ApiResponse<CommentPolicy>
            {
                IsSuccess = true,
                Message = "Comment policy added successfully",
                StatusCode = 201,
                ResponseObject = newComentPolicy
            };
        }

        public async Task<ApiResponse<CommentPolicy>> DeleteCommentPolicyByIdAsync(string commentPolicyId)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(commentPolicyId);
            if (commentPolicy == null)
            {
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Comment policy not found",
                    StatusCode = 404
                };
            }
            var deletedCommentPolicy = await _commentPolicyRepository.DeleteCommentPolicyByIdAsync(commentPolicyId);
            return new ApiResponse<CommentPolicy>
            {
                IsSuccess = true,
                Message = "Comment policy deleted successfully",
                StatusCode = 200,
                ResponseObject = deletedCommentPolicy
            };
        }

        public async Task<ApiResponse<CommentPolicy>> GetCommentPolicyByIdAsync(string commentPolicyId)
        {
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync(commentPolicyId);
            if (commentPolicy == null)
            {
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Comment policy not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<CommentPolicy>
            {
                IsSuccess = true,
                Message = "Comment policy deleted successfully",
                StatusCode = 200,
                ResponseObject = commentPolicy
            };
        }

        public async Task<ApiResponse<IEnumerable<CommentPolicy>>> GetCommentPoliciesAsync()
        {
            var commentPolicies = await _commentPolicyRepository.GetCommentPoliciesAsync();
            if (commentPolicies.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<CommentPolicy>>
                {
                    IsSuccess = true,
                    Message = "No comment policies found",
                    StatusCode = 200,
                    ResponseObject = commentPolicies
                };
            }
            return new ApiResponse<IEnumerable<CommentPolicy>>
            {
                IsSuccess = true,
                Message = "Comment policies found successfully",
                StatusCode = 200,
                ResponseObject = commentPolicies
            };
        }

        public async Task<ApiResponse<CommentPolicy>> UpdateCommentPolicyAsync(CommentPolicyDto commentPolicyDto)
        {
            if (commentPolicyDto.Id == null)
            {
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Comment policy id must not be null",
                    StatusCode = 400
                };
            }
            var policy = await _policyRepository.GetPolicyByIdAsync(commentPolicyDto.PolicyId);
            if (policy == null)
            {
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(commentPolicyDto.PolicyId);
            if (commentPolicy == null)
            {
                return new ApiResponse<CommentPolicy>
                {
                    IsSuccess = false,
                    Message = "Comment policy not found",
                    StatusCode = 404
                };
            }
            var updatedComentPolicy = await _commentPolicyRepository.UpdateCommentPolicyAsync(
                ConvertFromDto.ConvertFromCommentPolicyDto_Update(commentPolicyDto));
            return new ApiResponse<CommentPolicy>
            {
                IsSuccess = true,
                Message = "Comment policy updated successfully",
                StatusCode = 200,
                ResponseObject = updatedComentPolicy
            };
        }
    }
}
