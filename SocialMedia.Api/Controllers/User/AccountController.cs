using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs.Authentication.Login;
using SocialMedia.Api.Data.DTOs.Authentication.Register;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data;
using SocialMedia.Api.Service.AccountService.UserAccountService;
using SocialMedia.Api.Service.AccountService.UserRolesService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SendEmailService;
using System.IdentityModel.Tokens.Jwt;
using SocialMedia.Api.Data.Models.MessageModel;
using SocialMedia.Api.Service.AccountService.EmailService;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Service.AccountService.TokenService;
using SocialMedia.Api.Service.AccountService.TwoFactoAuthenticationService;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Service.AccountService.SettingsService;
using SocialMedia.Api.Data.DTOs.Authentication.ResetEmail;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Api.Data.DTOs.Authentication.UpdateAccount;


namespace SocialMedia.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IUserRolesService _rolesService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly ITwoFactoAuthenticationService _twoFactoAuthenticationService;
        private readonly ISettingsService _settingsService;  

        public AccountController(
            IUserAccountService _userAccountService, 
            IUserRolesService _rolesService,
            UserManager<SiteUser> _userManager, 
            ISendEmailService _sendEmailService,
            IEmailService _emailService, 
            ApplicationDbContext _dbContext,
            ITokenService _tokenService,
            UserManagerReturn _userManagerReturn,
            ITwoFactoAuthenticationService _twoFactoAuthenticationService,
            ISettingsService _settingsService)
        {
            this._userAccountService = _userAccountService;
            this._rolesService = _rolesService;
            this._userManager = _userManager;
            this._sendEmailService = _sendEmailService;
            this._emailService = _emailService;
            this._dbContext = _dbContext;
            this._tokenService = _tokenService;
            this._userManagerReturn = _userManagerReturn;
            this._twoFactoAuthenticationService = _twoFactoAuthenticationService;
            this._settingsService = _settingsService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("access-token")]
        public ActionResult<string> GetAccessToken()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
            {
                var token = HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
                return token!;
            }
            return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>._401_UnAuthorized());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("decode-JWT-token")]
        public ActionResult<object> DecodeAccessToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var decodedToken = DecodeJwt(jwtToken);
            return decodedToken!;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            try
            {
                var tokenResponse = await _userAccountService.CreateUserWithTokenAsync(registerDto);
                if (tokenResponse.IsSuccess && tokenResponse.ResponseObject != null)
                {
                    if (_dbContext.Roles.ToList().Count == 0 || _dbContext.Roles.ToList() == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                            ._500_ServerError("No roles found to assign to user"));
                    }
                    await _rolesService.AssignRolesToUserAsync(registerDto.Roles,
                        tokenResponse.ResponseObject.User);


                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                        new
                        {
                            token = tokenResponse.ResponseObject.Token,
                            userNameOrEmail = registerDto.Email
                        }, Request.Scheme);

                    var message = new Message(new string[] { registerDto.Email },
                        "Confirmation Email Link", confirmationLink!);
                    _sendEmailService.SendEmail(message);
                    string msg = $"Email confirmation link sent to your email please check your inbox and confirm your email";
                    return StatusCode(StatusCodes.Status200OK,
                        StatusCodeReturn<string>._200_Success(msg));
                }
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userNameOrEmail, string token)
        {
            var result = await _emailService.ConfirmEmail(userNameOrEmail, token);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            try
            {
                var loginResponse = await _userAccountService.LoginUserAsync(loginDto);
                if (loginResponse.ResponseObject != null)
                {
                    var user = loginResponse.ResponseObject.User;
                    if (user != null && user.Email != null)
                    {
                        if (user.TwoFactorEnabled)
                        {
                            var token = loginResponse.ResponseObject.Token;
                            var message = new Message(new string[] { user.Email },
                                "OTP code", token);
                            _sendEmailService.SendEmail(message);
                            return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<string>
                                ._200_Success("OTP sent successfully to your email"));
                        }
                    }
                    return Ok(loginResponse);
                }
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginTwoFactorAuthenticationAsync(string otp, string userNameOrEmail)
        {
            try
            {
                var response = await _userAccountService.LoginUserWithOTPAsync(otp, userNameOrEmail);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }


        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
            {
                await HttpContext.SignOutAsync();
                return StatusCode(StatusCodes.Status400BadRequest, StatusCodeReturn<string>
                    ._200_Success("Logged out successfully"));
            }
            return StatusCode(StatusCodes.Status400BadRequest, StatusCodeReturn<string>
                ._400_BadRequest("You are not logged in"));
        }

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
                return StatusCode(StatusCodes.Status404NotFound,
                    StatusCodeReturn<string>._404_NotFound("User not found"));
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                StatusCodeReturn<string>._401_UnAuthorized());
        }

        [HttpGet("Me/{userName}")]
        public async Task<IActionResult> GetUserByUserNameAsync([FromRoute] string userName)
        {
            try
            {
                var userByUserName = await _userManager.FindByNameAsync(userName);
                if (userByUserName == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("User not found"));
                }
                if (HttpContext.User != null && HttpContext.User.Identity != null &&
                HttpContext.User.Identity.Name != null)
                {
                    var loggedInUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (loggedInUser != null)
                    {

                        if (userByUserName.UserName == loggedInUser.UserName)
                        {
                            var userMapperWithRoles = ConvertToDto.FromUserToUserMapperDto(loggedInUser,
                            await _userManager.GetRolesAsync(loggedInUser));
                            return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<object>
                                ._200_Success("User found successfully", userMapperWithRoles));
                        }
                    }
                }
                var userMapperWithoutRoles = ConvertToDto.FromUserToUserMapperDto(userByUserName);
                return StatusCode(StatusCodes.Status200OK, StatusCodeReturn<object>
                       ._200_Success("User found successfully", userMapperWithoutRoles));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(LoginResponse tokens)
        {
            try
            {
                var response = await _tokenService.RenewAccessTokenAsync(tokens);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [HttpPost("enable-2FA")]
        public async Task<IActionResult> EnableTwoFactorAuthenticationAsync()
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(e.Message));
            }
        }

        [HttpPost("disable-2FA")]
        public async Task<IActionResult> DisableTwoFactorAuthenticationAsync()
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
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                    ._500_ServerError(e.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("generate-password-reset-object")]
        public ActionResult<object> GenerateResetPasswordObject(string email, string token)
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

        [AllowAnonymous]
        [HttpPost("resend-confirmation-email-link")]
        public async Task<IActionResult> ResendEmailConfirmationLinkAsync(string userNameOrEmail)
        {
            try
            {
                var response = await _tokenService.GenerateEmailConfirmationTokenAsync(userNameOrEmail);
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null && response.ResponseObject.User != null)
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet("generate-email-reset-object")]
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
        [HttpPost("reset-email-link")]
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

        [Authorize(Roles = "User")]
        [HttpPut("updateAccountInfo")]
        public async Task<IActionResult> UpdateAccountInfoAsync(
            [FromBody] UpdateAccountInfoDto updateAccountDto)
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-account-roles")]
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


        [HttpPut("lock-profile")]
        public async Task<IActionResult> LockProfileAsync()
        {
            try
            {
                if (HttpContext.User != null && HttpContext.User.Identity != null
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

        [HttpPut("unlock-profile")]
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

        [HttpPut("update-user-account-react-policy")]
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

        [HttpPut("update-user-account-friend-list-policy")]
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

        [HttpPut("update-user-account-posts-policy")]
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

        [HttpPut("update-user-account-comment-policy")]
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
        [HttpDelete("delete-account-link")]
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
    }
}
