

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GroupMemberRepository;
using SocialMedia.Api.Repository.GroupMemberRoleRepository;
using SocialMedia.Api.Repository.GroupRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.RoleRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.GroupManager;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Service.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IPolicyService _policyService;
        private readonly IRoleRepository _roleRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IGroupMemberRoleRepository _groupMemberRoleRepository;
        private readonly IGroupManager _groupManager;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IPolicyRepository _policyRepository;
        public GroupService(IGroupRepository _groupRepository, IPolicyService _policyService,
            IRoleRepository _roleRepository, IGroupManager _groupManager,
            IGroupMemberRepository _groupMemberRepository, UserManagerReturn _userManagerReturn, 
            IGroupMemberRoleRepository _groupMemberRoleRepository, IPolicyRepository _policyRepository)
        {
            this._groupRepository = _groupRepository;
            this._policyService = _policyService;
            this._roleRepository = _roleRepository;
            this._groupMemberRepository = _groupMemberRepository;
            this._groupMemberRoleRepository = _groupMemberRoleRepository;
            this._groupManager = _groupManager;
            this._userManagerReturn = _userManagerReturn;
            this._policyRepository = _policyRepository;
        }
        public async Task<ApiResponse<Group>> AddGroupAsync(AddGroupDto addGroupDto, SiteUser user)
        {
            var policy = await _policyService.GetPolicyByIdOrNameAsync(addGroupDto.GroupPolicyIdOrName);
            if(policy != null && policy.ResponseObject != null)
            {
                var adminRole = await _roleRepository.GetRoleByRoleNameAsync("admin");
                if (adminRole != null)
                {
                    addGroupDto.GroupPolicyIdOrName = policy.ResponseObject.Id;
                    return await CreateGroupWithMemberAdmin(addGroupDto, user, adminRole);
                }
                return StatusCodeReturn<Group>
                ._404_NotFound("Admin role not found");
            }
            return StatusCodeReturn<Group>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<Group>> DeleteGroupByIdAsync(string groupId, SiteUser user)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group != null)
            {
                if(group.CreatedUserId == user.Id)
                {
                    var groupMembers = await _groupMemberRepository.GetGroupMembersAsync(group.Id);
                    if (groupMembers.ToList().Count == 0)
                    {
                        await _groupRepository.DeleteByIdAsync(groupId);
                        group.User = _userManagerReturn.SetUserToReturn(user);
                        group.GroupPolicy = await _policyRepository.GetByIdAsync(group.GroupPolicyId);
                        return StatusCodeReturn<Group>
                            ._200_Success("Group deleted successfully", group);
                    }
                    return StatusCodeReturn<Group>
                    ._403_Forbidden("Group is not empty");
                }
                return StatusCodeReturn<Group>
                    ._403_Forbidden("Only allowd for group creator to delete it");
            }
            return StatusCodeReturn<Group>
                ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<IEnumerable<Group>>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
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
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group != null)
            {
                group.User = _userManagerReturn.SetUserToReturn(await _userManagerReturn
                    .GetUserByUserNameOrEmailOrIdAsync(group.CreatedUserId));
                group.GroupPolicy = await _policyRepository.GetByIdAsync(group.GroupPolicyId);
                return StatusCodeReturn<Group>
                    ._200_Success("Group found successfully", group);
            }
            return StatusCodeReturn<Group>
                ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<Group>> UpdateGroupAsync(UpdateGroupDto updateGroupDto, SiteUser user)
        {
            var group = await _groupRepository.GetByIdAsync(updateGroupDto.Id);
            if (group != null)
            {
                if(group.CreatedUserId == user.Id)
                {
                    var updatedGroup = await _groupRepository.UpdateAsync(
                    ConvertFromDto.ConvertFromGroupDto_Update(updateGroupDto, user, group));
                    updatedGroup.User = _userManagerReturn.SetUserToReturn(user);
                    updatedGroup.GroupPolicy = await _policyRepository.GetByIdAsync(group.GroupPolicyId);
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
            var group = await _groupRepository.GetByIdAsync(updateExistGroupPolicyDto.GroupId);
            if (group != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updateExistGroupPolicyDto.GroupPolicyIdOrName);
                if (policy != null && policy.ResponseObject != null)
                {
                    if(user.Id == group.CreatedUserId 
                        || (await _groupManager.IsInRoleAsync(user, group, "admin")).IsSuccess)
                    {
                        updateExistGroupPolicyDto.GroupPolicyIdOrName = policy.ResponseObject.Id;
                        var updatedGroup = await _groupRepository.UpdateAsync(ConvertFromDto
                            .ConvertFromGroupDto_Update(updateExistGroupPolicyDto, group));
                        updatedGroup.User = _userManagerReturn.SetUserToReturn(user);
                        updatedGroup.GroupPolicy = await _policyRepository.GetByIdAsync(group.GroupPolicyId);
                        return StatusCodeReturn<Group>
                            ._200_Success("Group policy updated successfully", updatedGroup);
                    }
                    return StatusCodeReturn<Group>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<Group>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<Group>
                            ._404_NotFound("Group not found");
        }


        private async Task<ApiResponse<Group>> CreateGroupWithMemberAdmin(AddGroupDto addGroupDto,
            SiteUser user, Role adminRole)
        {
            var newGroup = await _groupRepository.AddAsync(ConvertFromDto
                .ConvertFromGroupDto_Add(addGroupDto, user));
            var groupMember = new GroupMember
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = newGroup.Id,
                MemberId = newGroup.CreatedUserId
            };
            await _groupMemberRepository.AddAsync(groupMember);
            await _groupMemberRoleRepository.AddAsync(new GroupMemberRole
            {
                Id = Guid.NewGuid().ToString(),
                RoleId = adminRole.Id,
                GroupMemberId = groupMember.Id
            });
            newGroup.User = _userManagerReturn.SetUserToReturn(user);
            newGroup.GroupPolicy = await _policyRepository.GetByIdAsync(newGroup.GroupPolicyId);
            return StatusCodeReturn<Group>
                ._201_Created("Group created successfully", newGroup);
        }



    }
}
