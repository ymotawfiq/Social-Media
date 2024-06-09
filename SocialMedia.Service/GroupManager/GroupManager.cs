

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GroupAccessRequestRepository;
using SocialMedia.Repository.GroupMemberRepository;
using SocialMedia.Repository.GroupMemberRoleRepository;
using SocialMedia.Repository.GroupRepository;
using SocialMedia.Repository.GroupRoleRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.GroupManager
{
    public class GroupManager : IGroupManager
    {
        private readonly IGroupAccessRequestRepository _groupAccessRequestRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IGroupMemberRoleRepository _groupMemberRoleRepository;
        public GroupManager(IGroupAccessRequestRepository _groupAccessRequestRepository,
            IGroupRoleRepository _groupRoleRepository, IGroupRepository _groupRepository,
            IGroupMemberRepository _groupMemberRepository, UserManagerReturn _userManagerReturn,
            IGroupMemberRoleRepository _groupMemberRoleRepository)
        {
            this._groupAccessRequestRepository = _groupAccessRequestRepository;
            this._groupRepository = _groupRepository;
            this._groupRoleRepository = _groupRoleRepository;
            this._groupMemberRepository = _groupMemberRepository;
            this._userManagerReturn = _userManagerReturn;
            this._groupMemberRoleRepository = _groupMemberRoleRepository;
        }

        public async Task<ApiResponse<bool>> AcceptGroupRequestAsync(string requestId, SiteUser user)
        {
            var request = await _groupAccessRequestRepository.GetGroupAccessRequestByIdAsync(requestId);
            if (request != null)
            {
                var group = await _groupRepository.GetGroupByIdAsync(request.GroupId);
                var isAdmin = await IsInRoleAsync(user, group, "admin");
                if (isAdmin.ResponseObject)
                {
                    var userRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync("user");
                    if (userRole != null)
                    {
                        var groupMember = new GroupMember
                        {
                            Id = Guid.NewGuid().ToString(),
                            GroupId = group.Id,
                            MemberId = request.UserId,
                        };
                        await _groupMemberRepository.AddGroupMemberAsync(groupMember);
                        await _groupMemberRoleRepository.AddGroupMemberRoleAsync(new GroupMemberRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            GroupMemberId = groupMember.Id,
                            RoleId = userRole.Id
                        });
                        await _groupAccessRequestRepository.DeleteGroupAccessRequestByIdAsync(request.Id);
                        return StatusCodeReturn<bool>
                            ._200_Success("Request accepted successfully", true);
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("User role not found");
                }
                return isAdmin;
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Request not found");
        }

        public async Task<ApiResponse<bool>> AddToRoleAsync(
            SiteUser admin, SiteUser user, Group group, string role)
        {
            var groupMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, group.Id);
            if (groupMember != null)
            {
                var isAdmin = await IsInRoleAsync(admin, group, "admin");
                if (isAdmin != null && isAdmin.ResponseObject)
                {
                    var getRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(role);
                    if (getRole != null)
                    {
                        return await CheckRoleAndAddToUserAsync(groupMember, group, getRole);
                    }
                    return StatusCodeReturn<bool>
                        ._404_NotFound("Role not found");
                }
                return StatusCodeReturn<bool>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                        ._404_NotFound("Group member not found");
        }

        public async Task<ApiResponse<bool>> AddToRoleAsync(string groupMemberId, SiteUser user, string role)
        {
            var groupMember = await _groupMemberRepository.GetGroupMemberAsync(groupMemberId);
            if (groupMember != null)
            {
                var group = await _groupRepository.GetGroupByIdAsync(groupMember.GroupId);
                var isAdmin = await IsInRoleAsync(user, group, "admin");
                if (isAdmin.ResponseObject)
                {
                    var getRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(role);
                    if (getRole != null)
                    {
                        return await CheckRoleAndAddToUserAsync(groupMember, group, getRole);
                    }
                    return StatusCodeReturn<bool>
                        ._404_NotFound("Role not found");
                }
                return isAdmin;
            }
            return StatusCodeReturn<bool>
                            ._404_NotFound("Group member not found");
        }

        public async Task<ApiResponse<IEnumerable<GroupMember>>> GetGroupMembersAsync(string groupId)
        {
            var members = await _groupMemberRepository.GetGroupMembersAsync(groupId);
            if (members.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<GroupMember>>
                    ._200_Success("No members found", members);
            }
            return StatusCodeReturn<IEnumerable<GroupMember>>
                    ._200_Success("Members found successfully", members);
        }

        public async Task<ApiResponse<IEnumerable<GroupAccessRequest>>> GetRequestsAsync(
            string groupId, SiteUser user)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                var isAdmin = await IsInRoleAsync(user, group, "admin");
                if (isAdmin != null && isAdmin.ResponseObject)
                {
                    var requests = await _groupAccessRequestRepository.GetGroupAccessRequestsByGroupIdAsync(
                        groupId);
                    foreach(var r in requests)
                    {
                        SetNull(r);
                    }
                    if (requests.ToList().Count == 0)
                    {
                        return StatusCodeReturn<IEnumerable<GroupAccessRequest>>
                            ._200_Success("No request found", requests);
                    }
                    return StatusCodeReturn<IEnumerable<GroupAccessRequest>>
                            ._200_Success("Requests found successfully", requests);
                }
                return StatusCodeReturn<IEnumerable<GroupAccessRequest>>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<IEnumerable< GroupAccessRequest >>
                    ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<IEnumerable<string>>> GetUserRolesAsync(string userId, string groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                var groupMember = await _groupMemberRepository.GetGroupMemberAsync(userId, group.Id);
                if (groupMember != null)
                {
                    var userGroupRolesIds = await _groupMemberRoleRepository.GetMemberRolesAsync(
                        groupMember.Id);
                    List<string> roles = new();
                    foreach (var role in userGroupRolesIds)
                    {
                        roles.Add((await _groupRoleRepository.GetGroupRoleByIdAsync(role.RoleId)).RoleName);
                    }
                    return StatusCodeReturn<IEnumerable<string>>
                        ._200_Success("Roles found successfully", roles);
                }
                return StatusCodeReturn<IEnumerable<string>>
                    ._404_NotFound("Group member not found");
            }
            return StatusCodeReturn<IEnumerable<string>>
                    ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<bool>> IsExistInGroupAsync(SiteUser user, Group group)
        {
            var groupMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, group.Id);
            if (groupMember != null)
            {
                return StatusCodeReturn<bool>
                    ._200_Success("User is member of group", true);
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("User is not member in this group");
        }

        public async Task<ApiResponse<bool>> IsInRoleAsync(SiteUser user, Group group, string role)
        {
            var getRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(role);
            if (getRole != null)
            {
                var groupMember = await _groupMemberRepository.GetGroupMemberAsync(
                    user.Id, group.Id);
                if (groupMember != null)
                {
                    var memberRole = await _groupMemberRoleRepository.GetGroupMemberRoleAsync(
                        groupMember.Id, getRole.Id);
                    if (memberRole != null)
                    {
                        return StatusCodeReturn<bool>
                        ._200_Success("User is in role", true);
                    }
                    return StatusCodeReturn<bool>
                        ._404_NotFound("User not in role", false);
                }
                return StatusCodeReturn<bool>
                        ._404_NotFound("Group member not found", false);
            }
            return StatusCodeReturn<bool>
                        ._404_NotFound("Role not found", false);
        }

        public async Task<ApiResponse<bool>> RemoveFromGroupAsync(SiteUser admin, SiteUser user, Group group)
        {
            var groupMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, group.Id);
            return await RemoveFromGroupAsync(groupMember.Id, user);
        }

        public async Task<ApiResponse<bool>> RemoveFromGroupAsync(string groupMemberId, SiteUser user)
        {
            var groupMember = await _groupMemberRepository.GetGroupMemberAsync(groupMemberId);
            if (groupMember != null)
            {
                var group = await _groupRepository.GetGroupByIdAsync(groupMember.GroupId);
                var isAdmin = await IsInRoleAsync(user, group, "admin");
                if (isAdmin != null)
                {
                    await _groupMemberRepository.DeleteGroupMemberAsync(groupMemberId);
                    return StatusCodeReturn<bool>
                        ._200_Success("Member deleted successfully from group", true);
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("Group member not found", false);
        }

        public async Task<ApiResponse<bool>> RemoveFromRoleAsync(
            SiteUser admin, SiteUser user, Group group, string role)
        {
            var groupMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, group.Id);
            return await RemoveFromRoleAsync(groupMember.Id, admin, role);
        }

        public async Task<ApiResponse<bool>> RemoveFromRoleAsync(
            string groupMemberId, SiteUser user, string role)
        {
            var getRole = await _groupRoleRepository.GetGroupRoleByRoleNameAsync(role);
            if (getRole != null)
            {
                var groupMember = await _groupMemberRepository.GetGroupMemberAsync(groupMemberId);
                if (groupMember != null)
                {
                    var group = await _groupRepository.GetGroupByIdAsync(groupMember.GroupId);
                    var userRoles = await GetUserRolesAsync(groupMember.MemberId, group.Id);
                    if (userRoles != null)
                    {
                        return await CheckUserRoleAndRemoveFromAsync(user, group, getRole, userRoles,
                            groupMember);
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("No roles found for this user");
                }
                
                return StatusCodeReturn<bool>
                                ._404_NotFound("Group member not found");
            }
            return StatusCodeReturn<bool>
                                ._404_NotFound("Role not found");
        }


        public async Task<ApiResponse<IEnumerable<GroupMember>>> GetUserJoinedGroupsAsync(
            SiteUser routeUser, SiteUser currentUser)
        {
            if (routeUser.Id == currentUser.Id)
            {
                var joinedGroups = await _groupMemberRepository.GetUserJoinedGroupsAsync(routeUser.Id);
                foreach(var g in joinedGroups)
                {
                    SetNull(g);
                }
                if (joinedGroups.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<GroupMember>>
                        ._200_Success("No groups found", joinedGroups);
                }
                return StatusCodeReturn<IEnumerable<GroupMember>>
                        ._200_Success("Groups found successfully", joinedGroups);
            }
            return StatusCodeReturn<IEnumerable<GroupMember>>
                ._403_Forbidden();
        }

        private void SetNull(GroupAccessRequest groupAccessRequest)
        {
            groupAccessRequest.Group = null;
            groupAccessRequest.User = null;
        }

        private void SetNull(GroupMember groupMember)
        {
            groupMember.Group = null;
            groupMember.GroupMemberRoles = null;
            groupMember.User = null;
        }


        private async Task<ApiResponse<bool>> CheckUserRoleAndRemoveFromAsync(SiteUser user, Group group,
            GroupRole getRole, ApiResponse<IEnumerable<string>> userRoles, GroupMember groupMember)
        {
            if (userRoles.ResponseObject != null && userRoles.ResponseObject.ToList().Count > 1)
            {
                var isAdmin = await IsInRoleAsync(user, group, "admin");
                if (isAdmin.ResponseObject)
                {
                    var isInRole = await _groupMemberRoleRepository.GetGroupMemberRoleAsync(
                        groupMember.Id, getRole.Id);
                    if (isInRole != null)
                    {
                        await _groupMemberRoleRepository.DeleteGroupMemberRoleAsync(isInRole.Id);
                        return StatusCodeReturn<bool>
                            ._200_Success("Role deleted from user successfully", true);
                    }
                    return StatusCodeReturn<bool>
                        ._403_Forbidden("User not in role");
                }
                return isAdmin;
            }
            return StatusCodeReturn<bool>
                        ._403_Forbidden();
        }

        private async Task<ApiResponse<bool>> CheckRoleAndAddToUserAsync(GroupMember groupMember,
            Group group, GroupRole role)
        {
            var isMemberInRole = await IsInRoleAsync(
            await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(groupMember.MemberId),
            group, role.RoleName);
            if (!isMemberInRole.ResponseObject)
            {
                await _groupMemberRoleRepository.AddGroupMemberRoleAsync(new GroupMemberRole
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupMemberId = groupMember.Id,
                    RoleId = role.Id
                });
                return StatusCodeReturn<bool>
                    ._200_Success("Role added successfully to user", true);
            }
            return StatusCodeReturn<bool>
                ._403_Forbidden("Member already in role");
        }


    }
}
