

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GroupMemberRepository;
using SocialMedia.Repository.GroupMemberRoleRepository;
using SocialMedia.Repository.GroupPolicyRepository;
using SocialMedia.Repository.GroupRepository;
using SocialMedia.Repository.GroupRoleRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IPolicyService _policyService;
        private readonly IGroupPolicyRepository _groupPolicyRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IGroupMemberRoleRepository _groupMemberRoleRepository;
        public GroupService(IGroupRepository _groupRepository, IPolicyService _policyService,
            IGroupPolicyRepository _groupPolicyRepository, IGroupRoleRepository _groupRoleRepository,
            IGroupMemberRepository _groupMemberRepository, 
            IGroupMemberRoleRepository _groupMemberRoleRepository)
        {
            this._groupRepository = _groupRepository;
            this._policyService = _policyService;
            this._groupPolicyRepository = _groupPolicyRepository;
            this._groupRoleRepository = _groupRoleRepository;
            this._groupMemberRepository = _groupMemberRepository;
            this._groupMemberRoleRepository = _groupMemberRoleRepository;
        }
        public async Task<ApiResponse<Group>> AddGroupAsync(AddGroupDto addGroupDto, SiteUser user)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(addGroupDto.GroupPolicyIdOrName);
            if(policy!=null && policy.ResponseObject != null)
            {
                var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                    policy.ResponseObject.Id);
                if (groupPolicy != null)
                {
                    var adminRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync("admin");
                    if (adminRole != null)
                    {
                        addGroupDto.GroupPolicyIdOrName = groupPolicy.Id;
                        return await CreateGroupWithMemberAdmin(addGroupDto, user, adminRole);
                    }
                    return StatusCodeReturn<Group>
                    ._404_NotFound("Admin role not found");
                }
                return StatusCodeReturn<Group>
                    ._404_NotFound("Group policy not found");
            }
            return StatusCodeReturn<Group>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Group>> DeleteGroupByIdAsync(string groupId, SiteUser user)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                if(group.CreatedUserId == user.Id)
                {
                    var groupMembers = await _groupMemberRepository.GetGroupMembersAsync(group.Id);
                    if (groupMembers.ToList().Count == 0)
                    {
                        await _groupRepository.DeleteGroupByIdAsync(groupId);
                        SetNull(group);
                        return StatusCodeReturn<Group>
                            ._200_Success("Group deleted successfully", group);
                    }
                    return StatusCodeReturn<Group>
                    ._403_Forbidden("Group is not empty");
                }
                return StatusCodeReturn<Group>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<Group>
                ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<IEnumerable<Group>>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllGroupsAsync();
            foreach (var g in groups)
            {
                SetNull(g);
            }
            if (groups.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Group>>
                    ._200_Success("No groups found", groups);
            }
            return StatusCodeReturn<IEnumerable<Group>>
                    ._200_Success("Groups found successfully", groups);
        }

        public async Task<ApiResponse<IEnumerable<Group>>> GetAllGroupsByUserIdAsync(string userId)
        {
            var groups = await _groupRepository.GetAllGroupsByUserIdAsync(userId);
            foreach(var g in groups)
            {
                SetNull(g);
            }
            if (groups.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Group>>
                    ._200_Success("No groups found", groups);
            }
            return StatusCodeReturn<IEnumerable<Group>>
                    ._200_Success("Groups found successfully", groups);
        }

        public async Task<ApiResponse<Group>> GetGroupByIdAsync(string groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                SetNull(group);
                return StatusCodeReturn<Group>
                    ._200_Success("Group found successfully", group);
            }
            return StatusCodeReturn<Group>
                ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<Group>> UpdateGroupAsync(UpdateGroupDto updateGroupDto, SiteUser user)
        {
            var group = await _groupRepository.GetGroupByIdAsync(updateGroupDto.Id);
            if (group != null)
            {
                if(group.CreatedUserId == user.Id)
                {
                    var updatedGroup = await _groupRepository.UpdateGroupAsync(
                    ConvertFromDto.ConvertFromGroupDto_Update(updateGroupDto, user, group));
                    SetNull(updatedGroup);
                    return StatusCodeReturn<Group>
                        ._200_Success("Group updated successfully", updatedGroup);
                }
                return StatusCodeReturn<Group>
                ._403_Forbidden();
            }
            return StatusCodeReturn<Group>
                ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<Group>> UpdateGroupAsync(
            UpdateExistGroupPolicyDto updateExistGroupPolicyDto, SiteUser user)
        {
            var group = await _groupRepository.GetGroupByIdAsync(updateExistGroupPolicyDto.GroupId);
            if (group != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateExistGroupPolicyDto.GroupPolicyIdOrName);
                if (policy != null && policy.ResponseObject != null)
                {
                    var groupPolicy = await _groupPolicyRepository.GetGroupPolicyByPolicyIdAsync(
                        policy.ResponseObject.Id);
                    if (groupPolicy != null)
                    {
                        if(user.Id == group.CreatedUserId)
                        {
                            updateExistGroupPolicyDto.GroupPolicyIdOrName = groupPolicy.Id;
                            var updatedGroup = await _groupRepository.UpdateGroupAsync(ConvertFromDto
                                .ConvertFromGroupDto_Update(updateExistGroupPolicyDto, group));
                            SetNull(updatedGroup);
                            return StatusCodeReturn<Group>
                                ._200_Success("Group policy updated successfully", updatedGroup);
                        }
                        return StatusCodeReturn<Group>
                            ._403_Forbidden();
                    }
                    return StatusCodeReturn<Group>
                            ._404_NotFound("Group policy not found");
                }
                return StatusCodeReturn<Group>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<Group>
                            ._404_NotFound("Group not found");
        }


        private async Task<ApiResponse<Group>> CreateGroupWithMemberAdmin(AddGroupDto addGroupDto,
            SiteUser user, GroupRole adminRole)
        {
            var newGroup = await _groupRepository.AddGroupAsync(ConvertFromDto
                .ConvertFromGroupDto_Add(addGroupDto, user));
            var groupMember = new GroupMember
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = newGroup.Id,
                MemberId = newGroup.CreatedUserId
            };
            await _groupMemberRepository.AddGroupMemberAsync(groupMember);
            await _groupMemberRoleRepository.AddGroupMemberRoleAsync(new GroupMemberRole
            {
                Id = Guid.NewGuid().ToString(),
                RoleId = adminRole.Id,
                GroupMemberId = groupMember.Id
            });
            SetNull(newGroup);
            return StatusCodeReturn<Group>
                ._201_Created("Group created successfully", newGroup);
        }

        private Group SetNull(Group group)
        {
            group.User = null;
            group.GroupPolicy = null;
            group.GroupMembers = null;
            group.GroupAccessRequests = null;
            return group;
        }

    }
}
