using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using SocialMedia.Data.DTOs.Authentication.Login;
using SocialMedia.Data.DTOs.Authentication.Register;
using SocialMedia.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Data.DTOs.Authentication.User;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models.MessageModel;
using SocialMedia.Service.SendEmailService;
using SocialMedia.Service.UserAccountService;
using System.Security.Claims;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserManagement _userManagementService;
        private readonly IEmailService _emailService;
        private readonly UserManager<SiteUser> _userManager;
        public AccountController
            (
            IUserManagement _userManagementService,
            IEmailService _emailService,
            UserManager<SiteUser> _userManager
            )
        {
            this._userManagementService = _userManagementService;
            this._emailService = _emailService;
            this._userManager = _userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            try
            {
                var tokenResponse = await _userManagementService.CreateUserWithTokenAsync(registerDto);
                if (tokenResponse.IsSuccess && tokenResponse.ResponseObject != null)
                {
                    await _userManagementService.AssignRolesToUserAsync(registerDto.Roles,
                        tokenResponse.ResponseObject.User);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                        new
                        {
                            token = tokenResponse.ResponseObject.Token,
                            email = registerDto.Email
                        }, Request.Scheme);
                    var message = new Message(new string[] { registerDto.Email }, "Confirmation email link",
                        confirmationLink!);
                    _emailService.SendEmail(message);

                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Message = $"Email confirmation link sent to your email please check your inbox and confirm your email"
                    });
                }
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var result = await _userManagementService.ConfirmEmailAsync(email, token);
            return Ok(result);
        }



        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                var loginResponse = await _userManagementService.GetOtpByLoginAsync(loginDto);
                if (loginResponse.ResponseObject != null)
                {
                    var user = loginResponse.ResponseObject.User;
                    if (user != null)
                    {
                        if (user.TwoFactorEnabled)
                        {
                            var token = loginResponse.ResponseObject.Token;
                            var message = new Message(new string[] { user.Email },
                                "OTP code", token);
                            _emailService.SendEmail(message);
                            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                            {
                                StatusCode = 200,
                                IsSuccess = true,
                                Message = "OTP sent successfully to your email"
                            });
                        }
                    }
                    return Ok(loginResponse);
                }
                return Ok(loginResponse);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginTwoFactorAuthenticationAsync(string otp, string email)
        {
            try
            {
                var response = await _userManagementService.LoginUserWithJwtTokenAsync(otp, email);
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("logout")]
        public async Task<string> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return "Logout success";
        }

        [HttpGet("current-user")]
        public ClaimsPrincipal GetCurrentUser()
        {
            return HttpContext.User;
        }

        [HttpPost("enable-2FA")]
        public async Task<IActionResult> EnableTwoFactorAuthenticationAsync([FromBody]string email)
        {
            var response = await _userManagementService.EnableTwoFactorAuthenticationAsync(email);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(LoginResponse tokens)
        {
            try
            {
                var response = await _userManagementService.RenewAccessTokenAsync(tokens);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("reset-password-object")]
        public async Task<IActionResult> GenerateResetPasswordObject(string email,string token)
        {
            var resetPasswordObject = new ResetPasswordDto
            {
                Email = email,
                Token = token
            };
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<ResetPasswordDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Reset password object created",
                ResponseObject = resetPasswordObject
            });
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            try
            {
                var response = await _userManagementService.GenerateResetPasswordTokenAsync(email);
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null)
                    {
                        var forgerPasswordLink = Url.Action(nameof(GenerateResetPasswordObject), "Account",
                            new
                            {
                                email = response.ResponseObject.Email,
                                token = response.ResponseObject.Token
                            }, Request.Scheme);
                        var message = new Message(new string[] { response.ResponseObject.Email! },
                            "Forget password", forgerPasswordLink!);
                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Forget password link sent ssuccessfully to your email"
                        });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Can't send forget password link to email please try again"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var response = await _userManagementService.ResetPasswordAsync(resetPasswordDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("generateEmailResetObject")]
        public async Task<IActionResult> GenerateEmailResetObject(string oldEmail, string newEmail
            , string token)
        {
            var resetEmailObject = new ResetEmailDto
            {
                NewEmail = newEmail,
                Token = token,
                OldEmail = oldEmail
            };
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<ResetEmailDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Reset email object created",
                ResponseObject = resetEmailObject
            });
        }

        [HttpPost("sendEmailToResetEmail")]
        public async Task<IActionResult> SendEmailToResetEmailAsync(string oldEmail, string newEmail)
        {
            try
            {
                var response = await _userManagementService.GenerateResetEmailTokenAsync(
                new ResetEmailDto
                {
                    OldEmail = oldEmail,
                    NewEmail = newEmail
                });
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null)
                    {
                        var resetEmailLink = Url.Action(nameof(GenerateEmailResetObject), "Account",
                            new
                            {
                                oldEmail = oldEmail,
                                newEmail = newEmail,
                                token = response.ResponseObject.Token
                            }, Request.Scheme);
                        var message = new Message(new string[] { response.ResponseObject.OldEmail! },
                            "Reset email", resetEmailLink!);
                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Email rest link sent to your email"
                        });
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("reset-email")]
        public async Task<IActionResult> ResetEmailAsync([FromBody] ResetEmailDto resetEmailDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetEmailDto.OldEmail);
                if (user != null)
                {
                    await _userManager.ChangeEmailAsync(user, resetEmailDto.NewEmail, resetEmailDto.Token);
                    await _userManager.UpdateAsync(user);
                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Message = "Email changed successfully"
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Unable to reset email"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccountAsync(string email)
        {
            try
            {
                var response = await _userManagementService.DeleteAccountAsync(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

    }
}
