
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.Models.MessageModel;
using SocialMedia.Api.Service.AccountService.SettingsService;
using SocialMedia.Api.Service.AccountService.TokenService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SendEmailService;

namespace SocialMedia.Api.Controllers.User
{
    [ApiController]
    public class PasswordController : ControllerBase
    {

        private readonly ITokenService _tokenService;
        private readonly ISendEmailService _sendEmailService;
        private readonly ISettingsService _settingsService;
        public PasswordController(ITokenService _tokenService, ISendEmailService _sendEmailService, 
        ISettingsService _settingsService)
        {
            this._tokenService = _tokenService;
            this._sendEmailService = _sendEmailService;
            this._settingsService = _settingsService;
        }

        [AllowAnonymous]
        [HttpGet("generatePasswordResetObject")]
        public ActionResult<object> GenerateResetPasswordObject(string email,string token)
        {
            var resetPasswordObject = new ResetPasswordDto
            {
                Email = email,
                Token = token
            };
            return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<ResetPasswordDto>
                ._200_Success("Reset password object created", resetPasswordObject));
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            try
            {
                var response = await _tokenService.GenerateResetPasswordTokenAsync(email);
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null)
                    {

                        var forgerPasswordLink = Url.Action(nameof(GenerateResetPasswordObject), "Account",
                            new
                            {
                                token = response.ResponseObject.Token,
                                email = email
                            }, Request.Scheme);

                        var message = new Message(new string[] { response.ResponseObject.Email! },
                            "Forget password", forgerPasswordLink!);
                        _sendEmailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                            ._200_Success("Forget password link sent successfully to your email"));
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, StatusCodeReturn<string>
                    ._400_BadRequest("Can't send forget password link to email please try again"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var response = await _settingsService.ResetPasswordAsync(resetPasswordDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

    }
}