using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models.MessageModel;
using SocialMedia.Api.Service.AccountService.EmailService;
using SocialMedia.Api.Service.AccountService.TokenService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SendEmailService;

namespace SocialMedia.Api.Controllers.User
{
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly ISendEmailService _sendEmailService;
        private readonly UserManager<SiteUser> _userManager;
        public EmailController(IEmailService _emailService, ITokenService _tokenService,
            ISendEmailService _sendEmailService, UserManager<SiteUser> _userManager)
        {
            this._emailService = _emailService;
            this._tokenService = _tokenService;
            this._sendEmailService = _sendEmailService;
            this._userManager = _userManager;
        }
        [AllowAnonymous]
        [HttpPost("resendConfirmationEmailLink")]
        public async Task<IActionResult> ResendEmailConfirmationLinkAsync(string userNameOrEmail)
        {
            try
            {
                var response = await _tokenService.GenerateEmailConfirmationTokenAsync(userNameOrEmail);
                if (response.IsSuccess)
                {
                    if (response.ResponseObject!=null && response.ResponseObject.User != null)
                    {
                        /*
                         
                         https://localhost:8001/confirm-email?token=CfDJ8D8ZUwNOdqxJuCP%2FLCTx6y7Qr5XBtpH90XpV%2Bgp7VTrU%2Finuy6r8K7rPgzwPGV%2BCHMJwPsfZoFxvS%2FqJrf%2BDwMjAvXrMyc95c%2BkaqAxNa3rbbFxzD9n%2F4v%2BHBBW852FDhnV%2BXZJRjyun%2BRjep0C5LIy99KSJjVipQhn1uCDVNusWCPZwNf4E3MkJAd6TZbnYvk72RA5fUC58rrYz1a%2BkuzHPqcW6VVUZ3ZUegR%2BoPPsUeSEJtNlS2btU%2BsHy9LkuLg%3D%3D&userNameOrEmail=yousef12
                         */

                        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                            new
                            {
                                token = response.ResponseObject.Token,
                                userNameOrEmail = userNameOrEmail
                            }, Request.Scheme);

                        var message = new Message(new string[] { response.ResponseObject.User.Email! }
                        , "Confirm email link", confirmationLink!);

                        _sendEmailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                            ._200_Success("Email confirmation link resent successfully"));
                    }
                    
                }
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }
                [AllowAnonymous]
        [HttpGet("generateEmailResetObject")]
        public IActionResult GenerateEmailResetObject(string oldEmail, string newEmail
            , string token)
        {
            var resetEmailObject = new ResetEmailDto
            {
                NewEmail = newEmail,
                Token = token,
                OldEmail = oldEmail
            };
            return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<ResetEmailDto>
                ._200_Success("Reset email object created", resetEmailObject));
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("resetEmailLink")]
        public async Task<IActionResult> SendEmailToResetEmailAsync(ResetEmailObjectDto resetEmailObjectDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var response = await _tokenService.GenerateResetEmailTokenAsync(resetEmailObjectDto);
                    if (response.IsSuccess)
                    {
                        if (response.ResponseObject != null)
                        {
                            var resetEmailLink = Url.Action(nameof(GenerateEmailResetObject), "Account",
                                new
                                {
                                    token = response.ResponseObject.Token,
                                    oldEmail = resetEmailObjectDto.OldEmail,
                                    newEmail = resetEmailObjectDto.NewEmail
                                }, Request.Scheme);

                            var message = new Message(new string[] { response.ResponseObject.OldEmail! },
                                "Reset email", resetEmailLink!);
                            _sendEmailService.SendEmail(message);
                            return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                                ._200_Success("Email rest link sent to your email"));
                        }
                    }
                    return Ok(response);
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

        [Authorize(Roles = "Admin,User")]
        [HttpPost("reset-email")]
        public async Task<IActionResult> ResetEmailAsync([FromBody] ResetEmailDto resetEmailDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByEmailAsync(resetEmailDto.OldEmail);
                    if (user != null)
                    {
                        await _userManager.ChangeEmailAsync(user, resetEmailDto.NewEmail, resetEmailDto.Token);
                        await _userManager.UpdateAsync(user);
                        return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                            ._200_Success("Email changed successfully"));
                    }
                    return StatusCode(StatusCodes.Status400BadRequest, StatusCodeReturn<string>
                        ._400_BadRequest("Unable to reset email"));
                }
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
                {
                    StatusCode = 401,
                    IsSuccess = false,
                    Message = "Unauthorized"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(ex.Message));
            }
        }


        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userNameOrEmail, string token)
        {
            var result = await _emailService.ConfirmEmail(userNameOrEmail, token);
            return Ok(result);
        }

    }
}