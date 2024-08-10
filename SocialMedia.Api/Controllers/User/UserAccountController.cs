using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.DTOs.Authentication.Login;
using SocialMedia.Api.Data.DTOs.Authentication.Register;
using SocialMedia.Api.Data.DTOs.Mappers;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models.MessageModel;
using SocialMedia.Api.Service.AccountService.EmailService;
using SocialMedia.Api.Service.AccountService.UserAccountService;
using SocialMedia.Api.Service.AccountService.UserRolesService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.SendEmailService;

namespace SocialMedia.Api.Controllers.User
{
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IUserRolesService _rolesService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _dbContext;
        
        public UserAccountController(IUserAccountService _userAccountService, IUserRolesService _rolesService,
            UserManager<SiteUser> _userManager, ISendEmailService _sendEmailService,
            IEmailService _emailService, ApplicationDbContext _dbContext)
        {
            this._userAccountService = _userAccountService;
            this._rolesService = _rolesService;
            this._userManager = _userManager;
            this._sendEmailService = _sendEmailService;
            this._emailService = _emailService;
            this._dbContext = _dbContext;
        }
        [Authorize(Roles ="Admin")]
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
            //return GetEmailFromJwtPayload(token);
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
                    if(_dbContext.Roles.ToList().Count==0 || _dbContext.Roles.ToList() == null){
                        return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<string>
                            ._500_ServerError("No roles found to assign to user"));
                    }
                    await _rolesService.AssignRolesToUserAsync(registerDto.Roles,
                        tokenResponse.ResponseObject.User);


                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "UserAccount",
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
                    if (user != null && user.Email!=null)
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
            catch(Exception ex)
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
            catch(Exception ex)
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
                        
                        if(userByUserName.UserName == loggedInUser.UserName)
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
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
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