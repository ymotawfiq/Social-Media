
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Service.AccountService.TwoFactoAuthenticationService;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Controllers.User
{
    public class TwoFactorAuthenticationController : ControllerBase
    {
        private readonly UserManagerReturn _userManagerReturn;
        private readonly ITwoFactoAuthenticationService _twoFactoAuthenticationService;
        public TwoFactorAuthenticationController(UserManagerReturn _userManagerReturn,
        ITwoFactoAuthenticationService _twoFactoAuthenticationService)
        {
            this._userManagerReturn = _userManagerReturn;
            this._twoFactoAuthenticationService = _twoFactoAuthenticationService;
        }

        [HttpPost("enable-2FA")]
        public async Task<IActionResult> EnableTwoFactorAuthenticationAsync()
        {
            try{
                if(HttpContext.User!=null && HttpContext.User.Identity!=null
                    && HttpContext.User.Identity.Name!=null){
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if(user!=null){
                        var response = await _twoFactoAuthenticationService
                            .EnableTwoFactorAuthenticationAsync(user.Email!);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch(Exception e){
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(e.Message));
            }
        }

        [HttpPost("disable-2FA")]
        public async Task<IActionResult> DisableTwoFactorAuthenticationAsync()
        {
            try{
                if(HttpContext.User!=null && HttpContext.User.Identity!=null
                    && HttpContext.User.Identity.Name!=null){
                    var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                        HttpContext.User.Identity.Name);
                    if(user!=null){
                        var response = await _twoFactoAuthenticationService
                            .DisableTwoFactorAuthenticationAsync(user.Email!);
                        return Ok(response);
                    }
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>
                    ._401_UnAuthorized());
            }
            catch(Exception e){
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(e.Message));
            }
        }   
    }
}