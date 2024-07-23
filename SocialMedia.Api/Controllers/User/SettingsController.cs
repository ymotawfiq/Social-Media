
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Api.Data.DTOs.Authentication.UpdateAccount;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models.MessageModel;
using SocialMedia.Api.Service.AccountService.RolesService;
using SocialMedia.Api.Service.AccountService.SettingsService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SendEmailService;

namespace SocialMedia.Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly UserManagerReturn _userManagerReturn;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IRolesService _rolesService;
        private readonly ISettingsService _settingsService;
        private readonly ISendEmailService _sendEmailService;
        public SettingsController(UserManagerReturn _userManagerReturn, UserManager<SiteUser> _userManager,
        IRolesService _rolesService, ISettingsService _settingsService, ISendEmailService _sendEmailService)
        {
            this._userManagerReturn = _userManagerReturn;
            this._userManager = _userManager;
            this._rolesService = _rolesService;
            this._settingsService = _settingsService;
            this._sendEmailService = _sendEmailService;
        }
                [Authorize(Roles = "User")]
        [HttpPut("updateAccountInfo")]
        public async Task<IActionResult> UpdateAccountInfoAsync(
            [FromBody] UpdateAccountInfoDto updateAccountDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var loggedInUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if (loggedInUser != null)
                    {
                        loggedInUser.FirstName = updateAccountDto.FirstName;
                        loggedInUser.LastName = updateAccountDto.LastName;
                        loggedInUser.DisplayName = updateAccountDto.DisplayName;
                        await _userManager.UpdateAsync(loggedInUser);
                        return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                            ._200_Success("Account updated successfully"));
                    }
                }

                return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                    ._403_Forbidden());

            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("updateAccountRoles")]
        public async Task<IActionResult> UpdateAccountRolesAsync(
            [FromBody] UpdateAccountRolesDto updateAccountRolesDto)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                updateAccountRolesDto.UserNameOrEmail);
            if (user != null)
            {
                await _rolesService.AssignRolesToUserAsync(updateAccountRolesDto.Roles, user);
                await _userManager.UpdateAsync(user);
                return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                    ._200_Success("Account roles updated successfully"));
            }
            return StatusCode(StatusCodes.Status404NotFound,
                    StatusCodeReturn<string>._404_NotFound("User not found"));
        }


        [HttpPut("lockProfile")]
        public async Task<IActionResult> LockProfileAsync()
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _settingsService.LockProfileAsync(user);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpPut("unlockProfile")]
        public async Task<IActionResult> UnLockProfileAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _settingsService.UnLockProfileAsync(user);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateUserAccountReactPolicy")]
        public async Task<IActionResult> UpdateAccountReactPolicyAsync([FromBody] string policyIdOrName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _settingsService.UpdateAccountReactPolicyAsync(
                            user, policyIdOrName);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateUserAccountFriendListPolicy")]
        public async Task<IActionResult> UpdateAccountFriendListPolicyAsync([FromBody] string policyIdOrName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _settingsService.UpdateAccountFriendListPolicyAsync(
                            user, policyIdOrName);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateUserAccountPostsPolicy")]
        public async Task<IActionResult> UpdateAccountPostsPolicyAsync([FromBody] string policyIdOrName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _settingsService.UpdateAccountPostsPolicyAsync(
                            user, policyIdOrName);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpPut("updateUserAccountCommentPolicy")]
        public async Task<IActionResult> UpdateAccountCommentPolicyAsync([FromBody] string policyIdOrName)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var response = await _settingsService.UpdateAccountCommentPolicyAsync(
                            user, policyIdOrName);
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("deleteAccountLink")]
        public async Task<IActionResult> DeleteAccount1Async(string userNameOrEmail)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userNameOrEmail);
                    if (user != null && user.Email != null && loggedInUser != null)
                    {
                        if (user.Email == loggedInUser.Email)
                        {
                            var token = HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];

                            string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/" +
                                $"delete-account?userNameOrEmail={userNameOrEmail}&token={token}";
                            var message = new Message(new string[] { user.Email }, "Delete account", url);
                            _sendEmailService.SendEmail(message);
                            return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                                ._200_Success("Delete account email sent successfully to your email"));
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, 
                            StatusCodeReturn<string>._403_Forbidden());
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, 
                    StatusCodeReturn<string>._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync(string userNameOrEmail, string token)
        {
            try
            {
                bool checkToken = new JwtSecurityTokenHandler().CanReadToken(token);
                if (!checkToken)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, StatusCodeReturn<string>
                        ._400_BadRequest("Can't read token"));
                }
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userEmail = GetEmailFromJwtPayload(jwtToken);
                if (userEmail != null && !userEmail.IsNullOrEmpty())
                {
                    var userByToken = await _userManager.FindByEmailAsync(userEmail);
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        userNameOrEmail);
                    if (userByToken != null && user != null)
                    {
                        if (userByToken.UserName == user.UserName)
                        {
                            var response = await _settingsService.DeleteAccountAsync(userNameOrEmail);
                            return Ok(response);
                        }
                    }

                }
                return StatusCode(StatusCodes.Status401Unauthorized,
                    StatusCodeReturn<string>._401_UnAuthorized());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }
        private string GetEmailFromJwtPayload(JwtSecurityToken token)
        {
            
            var payload = token.Payload;
            var values = payload.Values;
            return values.First().ToString()!;
        }

    }
}