

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.GroupPolicyRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.GroupPolicyService
{
    public class GroupPolicyService : IGroupPolicyService
    {
        private readonly IGroupPolicyRepository _groupPolicyRepository;
        private readonly IPolicyService _policyService;
        public GroupPolicyService(IGroupPolicyRepository _groupPolicyRepository,
            IPolicyService _policyService)
        {
            this._groupPolicyRepository = _groupPolicyRepository;
            this._policyService = _policyService;
        }
        public async Task<object> AddGrouPolicyAsync(AddGroupPolicyDto addGroupPolicyDto)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(addGroupPolicyDto.PolicyIdOrName);
            if(policy!=null && policy.ResponseObject != null)
            {
                var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (groupPolicy == null)
                {
                    addGroupPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var newGroupPolicy = await _groupPolicyRepository.AddGroupPolicyAsync(
                        ConvertFromDto.ConvertFromGroupPolicyDto_Add(addGroupPolicyDto));
                    return StatusCodeReturn<object>
                        ._201_Created("Group policy added successfully", newGroupPolicy);
                }
                return StatusCodeReturn<object>
                        ._403_Forbidden("Group policy already exists");
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Policy not found");
        }

        public async Task<object> DeleteGrouPolicyByGroupPolicyIdOrPolicyIdOrNameAsync(
            string groupPolicyIdOrPolicyIdOrName)
        {
            var groupPolicy = await GetGroupPolicyAsync(groupPolicyIdOrPolicyIdOrName);
            if (groupPolicy.IsSuccess && groupPolicy.ResponseObject!=null)
            {
                await _groupPolicyRepository.DeleteGroupPolicyByIdAsync(groupPolicy.ResponseObject.Id);
                return StatusCodeReturn<object>
                    ._200_Success("Group policy deleted successfully", groupPolicy.ResponseObject);
            }
            return groupPolicy;
        }

        public async Task<object> DeleteGrouPolicyByIdAsync(string groupPolicyId)
        {
            var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByIdAsync(groupPolicyId);
            if (groupPolicy != null)
            {
                await _groupPolicyRepository.DeleteGroupPolicyByIdAsync(groupPolicyId);
                return StatusCodeReturn<object>
                    ._200_Success("Group policy deleted successfully", groupPolicy);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group policy not found");
        }

        public async Task<object> DeleteGrouPolicyByPolicyIdAsync(string policyId)
        {
            var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(policyId);
            if (groupPolicy != null)
            {
                await _groupPolicyRepository.DeleteGroupPolicyByIdAsync(policyId);
                return StatusCodeReturn<object>
                    ._200_Success("Group policy deleted successfully", groupPolicy);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group policy not found");
        }

        public async Task<object> DeleteGrouPolicyByPolicyIdOrNameAsync(string policyIdOrName)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (groupPolicy != null)
                {
                    await _groupPolicyRepository.DeleteGroupPolicyByPolicyIdAsync(policy.ResponseObject.Id);
                    return StatusCodeReturn<object>
                        ._200_Success("Group policy deleted successfully", groupPolicy);
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("Group policy not found");
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Policy not found");
        }

        public async Task<object> GetGrouPoliciesAsync()
        {
            var groupPolicies = await _groupPolicyRepository.GetGroupPoliciesAsync();
            if (groupPolicies.ToList().Count == 0)
            {
                return StatusCodeReturn<object>
                    ._200_Success("No group policies found", groupPolicies);
            }
            return StatusCodeReturn<object>
                    ._200_Success("Group policies found successfully", groupPolicies);
        }

        public async Task<object> GetGrouPolicyByGroupPolicyIdOrPolicyIdOrNameAsync(
            string groupPolicyIdOrPolicyIdOrName)
        {
            return await GetGroupPolicyAsync(groupPolicyIdOrPolicyIdOrName);
        }

        public async Task<object> GetGrouPolicyByIdAsync(string groupPolicyId)
        {
            var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByIdAsync(groupPolicyId);
            if (groupPolicy != null)
            {
                return StatusCodeReturn<object>
                    ._200_Success("Group policy found successfully", groupPolicy);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group policy not found");
        }

        public async Task<object> GetGrouPolicyByPolicyIdAsync(string policyId)
        {
            var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(policyId);
            if (groupPolicy != null)
            {
                return StatusCodeReturn<object>
                    ._200_Success("Group policy found successfully", groupPolicy);
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group policy not found");
        }

        public async Task<object> GetGrouPolicyByPolicyIdOrNameAsync(string policyIdOrName)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (groupPolicy != null)
                {
                    return StatusCodeReturn<object>
                        ._200_Success("Group policy found successfully", groupPolicy);
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("Group policy not found");
            }
            return StatusCodeReturn<object>
                    ._404_NotFound("Policy not found");
        }

        public async Task<object> UpdateGrouPolicyAsync(UpdateGroupPolicyDto updateGroupPolicyDto)
        {
            var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByIdAsync(updateGroupPolicyDto.Id);
            if (groupPolicy != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateGroupPolicyDto.PolicyIdOrName);
                if (policy != null && policy.ResponseObject != null)
                {
                    updateGroupPolicyDto.PolicyIdOrName = policy.ResponseObject.Id;
                    var existGroupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                        policy.ResponseObject.Id);
                    if (existGroupPolicy == null)
                    {
                        var updatedGroupPolicy = await _groupPolicyRepository.UpdateGroupPolicyAsync(
                            ConvertFromDto.ConvertFromGroupPolicyDto_Update(updateGroupPolicyDto));
                        return StatusCodeReturn<object>
                            ._200_Success("Group policy updated successfully", updatedGroupPolicy);
                    }
                    return StatusCodeReturn<object>
                        ._403_Forbidden("Group policy already exists");
                }
                return StatusCodeReturn<object>
                        ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<object>
                        ._404_NotFound("Group policy not found");
        }


        private async Task<ApiResponse<GroupPolicy>> GetGroupPolicyAsync(string groupPolicyIdOrPolicyIdOrName)
        {
            var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByIdAsync(
                groupPolicyIdOrPolicyIdOrName);
            if (groupPolicy != null)
            {
                return StatusCodeReturn<GroupPolicy>
                            ._200_Success("Group policy found successfully", groupPolicy);
            }
            var policy = await _policyService.GetPolicyByIdOrNameAsync(groupPolicyIdOrPolicyIdOrName);
            if (policy != null && policy.ResponseObject != null)
            {
                groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (groupPolicy != null)
                {
                    return StatusCodeReturn<GroupPolicy>
                        ._200_Success("Group policy found successfully", groupPolicy);
                }
                return StatusCodeReturn<GroupPolicy>
                        ._404_NotFound("Group policy not found");
            }
            return StatusCodeReturn<GroupPolicy>
                        ._404_NotFound("Policy not found");
        }


    }
}
