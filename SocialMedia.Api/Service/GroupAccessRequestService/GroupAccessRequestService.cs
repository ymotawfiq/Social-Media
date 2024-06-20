

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GroupAccessRequestRepository;
using SocialMedia.Api.Repository.GroupMemberRepository;
using SocialMedia.Api.Repository.GroupMemberRoleRepository;
using SocialMedia.Api.Repository.GroupRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.RoleRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.GroupAccessRequestService
{
    public class GroupAccessRequestService : IGroupAccessRequestService
    {
        private readonly IGroupAccessRequestRepository _groupAccessRequestRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IGroupMemberRoleRepository _groupMemberRoleRepository;
        public GroupAccessRequestService(IGroupAccessRequestRepository _groupAccessRequestRepository,
            IGroupMemberRepository _groupMemberRepository, IGroupRepository _groupRepository,
            IPolicyRepository _policyRepository,
            IRoleRepository _roleRepository, IGroupMemberRoleRepository _groupMemberRoleRepository)
        {
            this._groupAccessRequestRepository = _groupAccessRequestRepository;
            this._groupMemberRepository = _groupMemberRepository;
            this._groupRepository = _groupRepository;
            this._policyRepository = _policyRepository;
            this._roleRepository = _roleRepository;
            this._groupMemberRoleRepository = _groupMemberRoleRepository;

        }
        public async Task<ApiResponse<object>> AddGroupAccessRequestAsync(
                    AddGroupAccessRequestDto addGroupAccessRequestDto, SiteUser user)
        {
            var group = await _groupRepository.GetByIdAsync(addGroupAccessRequestDto.GroupId);
            if (group != null)
            {
                var isMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id,
                            addGroupAccessRequestDto.GroupId);
                if (isMember == null)
                {
                    var groupPolicy = await _policyRepository.GetByIdAsync(group.GroupPolicyId);
                    if (groupPolicy != null)
                    {
                        return await CheckGroupPolicyAndApplyRequestAsync(groupPolicy, group, user);
                    }
                    return StatusCodeReturn<object>
                        ._404_NotFound("Group policy not found");
                }
                return StatusCodeReturn<object>
                    ._403_Forbidden("You are member in group");
            }
            return StatusCodeReturn<object>
                        ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<GroupAccessRequest>> DeleteGroupAccessRequestAsync(
            string groupAccessRequestId, SiteUser user)
        {
            var request = await _groupAccessRequestRepository.GetByIdAsync(groupAccessRequestId);
            if (request != null)
            {
                if(request.UserId == user.Id)
                {
                    await _groupAccessRequestRepository.DeleteByIdAsync(groupAccessRequestId);
                    return StatusCodeReturn<GroupAccessRequest>
                        ._200_Success("Request deleted successfully", request);
                }
                return StatusCodeReturn<GroupAccessRequest>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<GroupAccessRequest>
                    ._404_NotFound("Request not found");
        }

        private async Task<ApiResponse<object>> CheckGroupPolicyAndApplyRequestAsync(
            Policy groupPolicy, Group group, SiteUser user)
        {
            if (groupPolicy.PolicyType == "PUBLIC")
            {
                var userRole = await _roleRepository.GetRoleByRoleNameAsync("user");
                if (userRole != null)
                {
                    var groupMember = new GroupMember
                    {
                        Id = Guid.NewGuid().ToString(),
                        GroupId = group.Id,
                        MemberId = user.Id,
                    };
                    await _groupMemberRepository.AddAsync(groupMember);
                    await _groupMemberRoleRepository.AddAsync(new GroupMemberRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        GroupMemberId = groupMember.Id,
                        RoleId = userRole.Id
                    });
                    return StatusCodeReturn<object>
                        ._201_Created("Joined successfully");
                }
                return StatusCodeReturn<object>
                    ._404_NotFound("User role not found");
            }
            else
            {
                var request = await _groupAccessRequestRepository.AddAsync(
                    new GroupAccessRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        GroupId = group.Id,
                        UserId = user.Id
                    });
                return StatusCodeReturn<object>
                    ._201_Created("Request sent successfully", request);
            }
        }


    }
}
