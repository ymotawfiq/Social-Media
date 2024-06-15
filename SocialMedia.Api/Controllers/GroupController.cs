using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.GroupAccessRequestService;
using SocialMedia.Service.GroupManager;
using SocialMedia.Service.GroupService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class GroupController : ControllerBase
    {

        private readonly IGroupService _groupService;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IGroupManager _groupManager;
        private readonly IGroupAccessRequestService _groupAccessRequestService;
        public GroupController(IGroupService _groupService, UserManagerReturn _userManagerReturn,
            IGroupManager _groupManager, IGroupAccessRequestService _groupAccessRequestService)
        {
            this._groupService = _groupService;
            this._userManagerReturn = _userManagerReturn;
            this._groupManager = _groupManager;
            this._groupAccessRequestService = _groupAccessRequestService;
        }

        [HttpPost("addGroup")]
        public async Task<IActionResult> AddGroupAsync([FromBody] AddGroupDto addGroupDto)
        {
            try
            {
                if(HttpContext.User != null && HttpContext.User.Identity != null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupService.AddGroupAsync(addGroupDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateGroup")]
        public async Task<IActionResult> UpdateGroupAsync([FromBody] UpdateGroupDto updateGroupDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupService.UpdateGroupAsync(updateGroupDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateExistGroupPolicy")]
        public async Task<IActionResult> UpdateExistGroupPolicyAsync(
            [FromBody] UpdateExistGroupPolicyDto updateExistGroupPolicyDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupService.UpdateGroupAsync(updateExistGroupPolicyDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetGroupByIdAsync([FromRoute] string groupId)
        {
            try
            {
                var response = await _groupService.GetGroupByIdAsync(groupId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroup/{groupId}")]
        public async Task<IActionResult> DeleteGroupByIdAsync([FromRoute] string groupId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupService.DeleteGroupByIdAsync(groupId, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


        [HttpGet("getUserGroups")]
        public async Task<IActionResult> GetUserGroupsAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupService.GetAllGroupsByUserIdAsync(user.Id);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


        [HttpPost("addGroupRequest")]
        public async Task<IActionResult> AddGroupRequestAsync(
            [FromBody] AddGroupAccessRequestDto addGroupAccessRequestDto)
        {
            try
            {
                if(HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupAccessRequestService.AddGroupAccessRequestAsync(
                            addGroupAccessRequestDto, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteGroupRequest/{groupRequestId}")]
        public async Task<IActionResult> DeleteGroupRequestAsync([FromRoute] string groupRequestId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupAccessRequestService.DeleteGroupAccessRequestAsync(
                            groupRequestId, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpPost("acceptGroupRequest/{requestId}")]
        public async Task<IActionResult> AcceptGroupRequestAsync([FromRoute] string requestId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupManager.AcceptGroupRequestAsync(requestId, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("deleteFromGroup/{groupMemberId}")]
        public async Task<IActionResult> DeleteFromGroupAsync([FromRoute] string groupMemberId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupManager.RemoveFromGroupAsync(
                            groupMemberId, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


        [HttpPost("addToRole")]
        public async Task<IActionResult> AddToRoleAsync(string groupMemberId, string role)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupManager.AddToRoleAsync(groupMemberId, user, role);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


        [HttpDelete("deleteFromRole")]
        public async Task<IActionResult> DeleteFromGroupAsync(string groupMemberId, string role)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupManager.RemoveFromRoleAsync(
                            groupMemberId, user, role);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


        [HttpGet("getGroupAccessRequests/{groupId}")]
        public async Task<IActionResult> GetGroupAccessRequestsAsync([FromRoute] string groupId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupManager.GetRequestsAsync(groupId, user);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }

        [HttpGet("getGroupMembers/{groupId}")]
        public async Task<IActionResult> GetGroupMembersAsync([FromRoute] string groupId)
        {
            var response = await _groupManager.GetGroupMembersAsync(groupId);
            return Ok(response);
        }

        [HttpGet("getJoinedGroups")]
        public async Task<IActionResult> GetJoinedGroupsAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (currentUser != null)
                    {
                        var response = await _groupManager.GetUserJoinedGroupsAsync(currentUser);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                        ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


        [HttpGet("getMemberRoles")]
        public async Task<IActionResult> GetUserRolesAsync(string userIdOrName, string groupId)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userIdOrName);
            if (user != null)
            {
                var response = await _groupManager.GetUserRolesAsync(user.Id, groupId);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                ._404_NotFound("User not found"));
        }

        [HttpGet("getCurrentMemberRoles/{groupId}")]
        public async Task<IActionResult> GetCurrentMemberRolesAsync(string groupId)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _groupManager.GetUserRolesAsync(user.Id, groupId);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                            ._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                        ._500_ServerError(ex.Message));
            }
        }


    }
}
