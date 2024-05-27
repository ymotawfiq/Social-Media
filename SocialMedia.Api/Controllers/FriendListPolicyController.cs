using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.FriendListPolicyRepository;
using SocialMedia.Service.FriendListPolicyService;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{

    [ApiController]
    public class FriendListPolicyController : ControllerBase
    {
        private readonly IFriendListPolicyService _friendListPolicyService;
        public FriendListPolicyController(IFriendListPolicyService _friendListPolicyService)
        {
            this._friendListPolicyService = _friendListPolicyService;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("getFriendListPolicy/{friendListPolicyIdOrPolicyName}")]
        public async Task<IActionResult> GetFriendListPolicyAsync(string friendListPolicyIdOrPolicyName)
        {
            try
            {
                var response = await _friendListPolicyService.GetFriendListPolicyAsync(
                    friendListPolicyIdOrPolicyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteFriendListPolicy/{friendListPolicyIdOrPolicyName}")]
        public async Task<IActionResult> DeleteFriendListPolicyAsync(string friendListPolicyIdOrPolicyName)
        {
            try
            {
                var response = await _friendListPolicyService.DeleteFriendListPolicyAsync(
                    friendListPolicyIdOrPolicyName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateFriendListPolicy")]
        public async Task<IActionResult> UpdateFriendListPolicyAsync([FromBody]
        UpdateFriendListPolicyDto updateFriendListPolicyDto)
        {
            try
            {
                var response = await _friendListPolicyService.UpdateFriendListPolicyAsync(
                    updateFriendListPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addFriendListPolicy")]
        public async Task<IActionResult> AddFriendListPolicyAsync([FromBody]
        AddFriendListPolicyDto addFriendListPolicyDto)
        {
            try
            {
                var response = await _friendListPolicyService.AddFriendListPolicyAsync(
                    addFriendListPolicyDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("friendListPolicies")]
        public async Task<IActionResult> GetFriendListPoliciesAsync()
        {
            try
            {
                var response = await _friendListPolicyService.GetFriendListPoliciesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }






    }
}
