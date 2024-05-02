using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SocialMedia.Data.DTOs.Authentication.Login;
using SocialMedia.Data.DTOs.Authentication.Register;
using SocialMedia.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Data.DTOs.Authentication.UpdateAccount;
using SocialMedia.Data.DTOs.Authentication.User;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models.MessageModel;
using SocialMedia.Service.SendEmailService;
using SocialMedia.Service.UserAccountService;
using System.IdentityModel.Tokens.Jwt;
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

        [Authorize(Roles ="Admin")]
        [HttpGet("accessToken")]
        public async Task<ActionResult<string>> GetAccessToken()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
            {
                var token = HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
                return token!;
            }
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
            {
                StatusCode = 401,
                IsSuccess = false,
                Message = "Unauthorized"
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("decodeJWTToken")]
        public async Task<ActionResult<object>> DecodeAccessToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var decodedToken = DecodeJwt(jwtToken);
            return decodedToken!;
            //return GetEmailFromJwtPayload(token);
        }

        [AllowAnonymous]
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
                            userNameOrEmail = registerDto.Email
                        }, Request.Scheme);

                    var message = new Message(new string[] { registerDto.Email },
                        "Confirmation Email Link", confirmationLink!);
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

        [AllowAnonymous]
        [HttpPost("resendConfirmationEmailLink")]
        public async Task<IActionResult> ResendEmailConfirmationLinkAsync(string userNameOrEmail)
        {
            try
            {
                var response = await _userManagementService.GenerateEmailConfirmationTokenAsync(userNameOrEmail);
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

                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Email confirmation link resent successfully"
                        });
                    }
                    
                }
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

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userNameOrEmail, string token)
        {
            var result = await _userManagementService.ConfirmEmail(userNameOrEmail, token);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                var loginResponse = await _userManagementService.LoginUserAsync(loginDto);
                if (loginResponse.ResponseObject != null)
                {
                    var user = loginResponse.ResponseObject.User;
                    if (user != null && user.Email!=null)
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

        [AllowAnonymous]
        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginTwoFactorAuthenticationAsync(string otp, string userNameOrEmail)
        {
            try
            {
                var response = await _userManagementService.LoginUserWithOTPAsync(otp, userNameOrEmail);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
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
        public async Task<IActionResult> LogoutAsync()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
            {
                await HttpContext.SignOutAsync();
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    Message = "Logged out successfully",
                    IsSuccess = true,
                    StatusCode = 200
                });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = "You are not logged in"
            });
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null &&
                HttpContext.User.Identity.Name != null)
            {
                var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (currentUser != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<object>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Message = "User founded successfully",
                        ResponseObject = new
                        {
                            DisplayName = currentUser.DisplayName,
                            FirstName = currentUser.FirstName,
                            Email = currentUser.Email,
                            LastName = currentUser.LastName,
                            PhoneNumber = currentUser.PhoneNumber,
                            UserName = currentUser.UserName
                        }
                    });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "User not found"
                });
            }
            return RedirectPermanent("/error/401");
        }

        [HttpGet("Me/{userName}")]
        public async Task<IActionResult> GetUserByUserNameAsync([FromRoute] string userName)
        {
            try
            {
                var userByUserName = await _userManager.FindByNameAsync(userName);
                if (userByUserName == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = "User not found"
                    });
                }
                if (HttpContext.User != null && HttpContext.User.Identity != null &&
                HttpContext.User.Identity.Name != null)
                {
                    var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (loggedInUser != null)
                    {
                        
                        if(userByUserName.UserName == loggedInUser.UserName)
                        {
                            return StatusCode(StatusCodes.Status200OK, new ApiResponse<object>
                            {
                                StatusCode = 200,
                                IsSuccess = true,
                                Message = "User founded successfully",
                                ResponseObject = new
                                {
                                    DisplayName = loggedInUser.DisplayName,
                                    FirstName = loggedInUser.FirstName,
                                    Email = loggedInUser.Email,
                                    LastName = loggedInUser.LastName,
                                    PhoneNumber = loggedInUser.PhoneNumber,
                                    UserName = loggedInUser.UserName,
                                    roles = await _userManager.GetRolesAsync(loggedInUser)
                                }
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new ApiResponse<object>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "User founded successfully",
                    ResponseObject = new
                    {
                        DisplayName = userByUserName.DisplayName,
                        FirstName = userByUserName.FirstName,
                        LastName = userByUserName.LastName,
                        UserName = userByUserName.UserName
                    }
                });
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

        [Authorize(Roles ="Admin,User")]
        [HttpPost("enable-2FA-byEmail")]
        public async Task<IActionResult> EnableTwoFactorAuthenticationByEmailAsync([FromBody]string email)
        {
            var response = await _userManagementService.EnableTwoFactorAuthenticationAsync(email);
            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("enable-2FA-byUserName")]
        public async Task<IActionResult> EnableTwoFactorAuthenticationByUserNameAsync([FromBody] string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var response = await _userManagementService.EnableTwoFactorAuthenticationAsync(user.Email!);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
            {
                StatusCode = 404,
                IsSuccess = false,
                Message = "User name not found"
            });
        }

        [Authorize(Roles = "Admin,User")]
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

        [AllowAnonymous]
        [HttpGet("generatePasswordResetObject")]
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

        [AllowAnonymous]
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
                                token = response.ResponseObject.Token,
                                email = email
                            }, Request.Scheme);

                        var message = new Message(new string[] { response.ResponseObject.Email! },
                            "Forget password", forgerPasswordLink!);
                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Forget password link sent successfully to your email"
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

        [Authorize(Roles = "Admin,User")]
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

        [AllowAnonymous]
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

        [Authorize(Roles = "Admin,User")]
        [HttpPost("resetEmailLink")]
        public async Task<IActionResult> SendEmailToResetEmailAsync(ResetEmailObjectDto resetEmailObjectDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var response = await _userManagementService.GenerateResetEmailTokenAsync(resetEmailObjectDto);
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
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
                {
                    StatusCode = 401,
                    IsSuccess = true,
                    Message = "Unauthorized"
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
                return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
                {
                    StatusCode = 401,
                    IsSuccess = false,
                    Message = "Unauthorized"
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

        [Authorize(Roles = "Admin,User")]
        [HttpPut("updateAccountInfo")]
        public async Task<IActionResult> UpdateAccountInfoAsync(
            [FromBody] UpdateAccountInfoDto updateAccountDto)
        {
            try
            {
                if(HttpContext.User!=null && HttpContext.User.Identity!=null 
                    && HttpContext.User.Identity.Name != null)
                {
                    var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (loggedInUser != null)
                    {
                        loggedInUser.FirstName = updateAccountDto.FirstName;
                        loggedInUser.LastName = updateAccountDto.LastName;
                        loggedInUser.DisplayName = updateAccountDto.DisplayName;
                        await _userManager.UpdateAsync(loggedInUser);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Account updated successfully"
                        });
                    }
                }

                return RedirectPermanent("/error/403");

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

        [Authorize(Roles = "Admin")]
        [HttpPut("updateAccountRoles")]
        public async Task<IActionResult> UpdateAccountRolesAsync(
            [FromBody] UpdateAccountRolesDto updateAccountRolesDto)
        {
            var user = await GetUserByUserNameOrEmailAsync(updateAccountRolesDto.UserNameOrEmail);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Account not found"
                });
            }
            await _userManagementService.AssignRolesToUserAsync(updateAccountRolesDto.Roles, user);
            await _userManager.UpdateAsync(user);
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Account roles updated successfully"
            });
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
                    var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
                    if (user != null && user.Email != null && loggedInUser != null)
                    {
                        if (user.Email == loggedInUser.Email)
                        {
                            var token = HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];

                            string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/" +
                                $"delete-account?userNameOrEmail={userNameOrEmail}&token={token}";
                            var message = new Message(new string[] { user.Email }, "Delete account", url);
                            _emailService.SendEmail(message);
                            return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                            {
                                StatusCode = 200,
                                IsSuccess = true,
                                Message = "Delete account email sent successfully to your email"
                            });
                        }
                        return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
                        {
                            StatusCode = 403,
                            IsSuccess = false,
                            Message = "Forbidden"
                        });
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = "User not found"
                    });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
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
                    return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                    {
                        StatusCode = 400,
                        IsSuccess = false,
                        Message = "Can't read token"
                    });
                }
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userEmail = GetEmailFromJwtPayload(jwtToken);
                if (userEmail != null && !userEmail.ToString().IsNullOrEmpty())
                {
                    var userByToken = await _userManager.FindByEmailAsync(userEmail);
                    var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
                    if (userByToken != null && user != null)
                    {
                        if (userByToken.UserName == user.UserName)
                        {
                            var response = await _userManagementService.DeleteAccountAsync(userNameOrEmail);
                            return Ok(response);
                        }
                    }

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
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


        #region Private Methods

        private async Task<SiteUser> GetUserByUserNameOrEmailAsync(string userNameOrEmail)
        {
            var userByEmail = await _userManager.FindByEmailAsync(userNameOrEmail);
            var userByName = await _userManager.FindByNameAsync(userNameOrEmail);
            if (userByEmail == null && userByName != null)
            {
                return userByName;
            }
            else if (userByName == null && userByEmail != null)
            {
                return userByEmail;
            }
            return null!;
        }

        private object DecodeJwt(JwtSecurityToken token)
        {
            var keyId = token.Header.Kid;
            var audience = token.Audiences.ToList();
            var claims = token.Claims.Select(claim => (claim.Type, claim.Value)).ToList();
            return new
            {
                keyId,
                token.Issuer,
                audience,
                claims,
                token.ValidTo,
                token.SignatureAlgorithm,
                token.RawData,
                token.Subject,
                token.ValidFrom,
                token.Header,
                token.Payload
            };
              
            
        }

        private string GetEmailFromJwtPayload(JwtSecurityToken token)
        {
            
            var payload = token.Payload;
            var values = payload.Values;
            return values.First().ToString()!;
        }

        /*
         
         "payload": 
        {
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "ymotawfiq@gmail.com",
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": "yousef12",
            "jti": "12f11ec3-866b-4b9b-914a-058ecccd3f45",
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "User",
            "exp": 1714209353,
            "iss": "https://localhost:8001",
            "aud": "https://localhost:8001"
         }
         */

        #endregion
    }
}
