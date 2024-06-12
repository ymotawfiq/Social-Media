

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.PostsPolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.PostsPolicyService
{
    public class PostsPolicyService : IPostsPolicyService
    {
        private readonly IPostsPolicyRepository _postsPolicyRepository;
        private readonly IPolicyService _policyService;
        public PostsPolicyService(IPostsPolicyRepository _postsPolicyRepository,
            IPolicyService _policyService)
        {
            this._postsPolicyRepository = _postsPolicyRepository;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<PostsPolicy>> AddAccountPostPolicyAsync
            (AddAccountPostsPolicyDto addAccountPostsPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(
                addAccountPostsPolicyDto.PolicyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var accountPostPolicy = await _postsPolicyRepository
                    .GetPostPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPostPolicy == null)
                {
                    addAccountPostsPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var newAccountPostsPolicy = await _postsPolicyRepository.AddPostPolicyAsync(
                        ConvertFromDto.ConvertAccountPostsPolicyDto_Add(addAccountPostsPolicyDto));
                    return StatusCodeReturn<PostsPolicy>
                        ._201_Created("Account posts policy created successfully", newAccountPostsPolicy);
                }
                return StatusCodeReturn<PostsPolicy>
                        ._403_Forbidden("Account post policy already exists");
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<PostsPolicy>> DeleteAccountPostPolicyAsync(
            string postPolicyIdOrPolicyIdOrPolicyName)
        {
            var accountPostsPolicy = await GetAccountPostsPolicyByIdOrPolicyAsync(
                postPolicyIdOrPolicyIdOrPolicyName);
            if (accountPostsPolicy != null)
            {
                await _postsPolicyRepository.DeletePostPolicyByIdAsync(accountPostsPolicy.Id);
                return StatusCodeReturn<PostsPolicy>
                    ._200_Success("Account post policy deleted successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<PostsPolicy>> DeleteAccountPostPolicyByIdAsync(
            string postPolicyId)
        {
            var accountPostsPolicy = await _postsPolicyRepository.GetPostPolicyByIdAsync(
                postPolicyId);
            if (accountPostsPolicy != null)
            {
                await _postsPolicyRepository.DeletePostPolicyByIdAsync(accountPostsPolicy.Id);
                return StatusCodeReturn<PostsPolicy>
                    ._200_Success("Account post policy deleted successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<PostsPolicy>> DeleteAccountPostPolicyByPolicyIdAsync
            (string policyId)
        {
            var accountPostsPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(
                policyId);
            if (accountPostsPolicy != null)
            {
                await _postsPolicyRepository.DeletePostPolicyByIdAsync(accountPostsPolicy.Id);
                return StatusCodeReturn<PostsPolicy>
                    ._200_Success("Account post policy deleted successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<IEnumerable<PostsPolicy>>> GetAccountPostPoliciesAsync()
        {
            var accountPostsPolicies = await _postsPolicyRepository.GetPostPoliciesAsync();
            if (accountPostsPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostsPolicy>>
                    ._200_Success("No account posts policy found", accountPostsPolicies);
            }
            return StatusCodeReturn<IEnumerable<PostsPolicy>>
                    ._200_Success("Account posts policy found successfully", accountPostsPolicies);
        }

        public async Task<ApiResponse<PostsPolicy>> GetAccountPostPolicyAsync(
            string postPolicyIdOrPolicyIdOrPolicyName)
        {
            var accountPostsPolicy = await GetAccountPostsPolicyByIdOrPolicyAsync(
                postPolicyIdOrPolicyIdOrPolicyName);
            if (accountPostsPolicy != null)
            {
                return StatusCodeReturn<PostsPolicy>
                    ._200_Success("Account post policy found successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<PostsPolicy>> GetAccountPostPolicyByIdAsync(string postPolicyId)
        {
            var accountPostsPolicy = await _postsPolicyRepository.GetPostPolicyByIdAsync(
                postPolicyId);
            if (accountPostsPolicy != null)
            {
                return StatusCodeReturn<PostsPolicy>
                    ._200_Success("Account post policy found successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<PostsPolicy>> GetAccountPostPolicyByPolicyIdAsync(string policyId)
        {
            var accountPostsPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(
                policyId);
            if (accountPostsPolicy != null)
            {
                return StatusCodeReturn<PostsPolicy>
                    ._200_Success("Account post policy found successfully", accountPostsPolicy);
            }
            return StatusCodeReturn<PostsPolicy>
                ._404_NotFound("Account post policy not found");
        }

        public async Task<ApiResponse<PostsPolicy>> UpdateAccountPostPolicyAsync(
            UpdateAccountPostsPolicyDto updateAccountPostsPolicyDto)
        {
            var accountPostsPolicy = await _postsPolicyRepository.GetPostPolicyByIdAsync(
                updateAccountPostsPolicyDto.Id);
            if (accountPostsPolicy != null)
            {
                var policy = await _policyService
                .GetPolicyByIdOrNameAsync(updateAccountPostsPolicyDto.PolicyIdOrName);
                if (policy != null && policy.ResponseObject != null)
                {
                    var existAccountPostsPolicy = await _postsPolicyRepository
                    .GetPostPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    if (existAccountPostsPolicy == null)
                    {
                        updateAccountPostsPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                        var updated = await _postsPolicyRepository.UpdatePostPolicyAsync(
                            ConvertFromDto.ConvertAccountPostsPolicyDto_Update(updateAccountPostsPolicyDto)
                            );
                        return StatusCodeReturn<PostsPolicy>
                            ._200_Success("Account post policy updated successfully", updated);
                    }
                    return StatusCodeReturn<PostsPolicy>
                        ._403_Forbidden("Account post policy already exists");
                }
            }
            return StatusCodeReturn<PostsPolicy>
                    ._404_NotFound("Account post policy not found");
        }

        private async Task<PostsPolicy> GetAccountPostsPolicyByIdOrPolicyAsync
            (string postPolicyIdOrPolicyIdOrPolicyName)
        {
            var accountPostsPolicyById = await _postsPolicyRepository.GetPostPolicyByIdAsync(
                postPolicyIdOrPolicyIdOrPolicyName);
            var accountPostsPolicyByPolicyId = await _postsPolicyRepository
                .GetPostPolicyByPolicyIdAsync(postPolicyIdOrPolicyIdOrPolicyName);
            if (accountPostsPolicyById != null)
            {
                return accountPostsPolicyById;
            }
            else if (accountPostsPolicyByPolicyId != null)
            {
                return accountPostsPolicyByPolicyId;
            }
            var policy = await _policyService.GetPolicyByIdOrNameAsync(postPolicyIdOrPolicyIdOrPolicyName);
            if (policy != null && policy.ResponseObject!=null)
            {
                accountPostsPolicyByPolicyId = await _postsPolicyRepository
                    .GetPostPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                if (accountPostsPolicyByPolicyId != null)
                {
                    return accountPostsPolicyByPolicyId;
                }
            }
            return null!;
        }
    }
}
