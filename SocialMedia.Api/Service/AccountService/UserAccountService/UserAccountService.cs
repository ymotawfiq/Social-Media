
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Api.Data.DTOs.Authentication.Login;
using SocialMedia.Api.Data.DTOs.Authentication.Register;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.AccountService.TokenService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Service.AccountService.UserAccountService
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly IPolicyService _policyService;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly SignInManager<SiteUser> _signInManager;
        private readonly ITokenService _tokenService;
        public UserAccountService(UserManager<SiteUser> _userManager, IPolicyService _policyService, 
            UserManagerReturn _userManagerReturn, SignInManager<SiteUser> _signInManager, 
            ITokenService _tokenService)
        {
            this._policyService = _policyService;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._userManagerReturn = _userManagerReturn;
            this._tokenService = _tokenService;
        }
        public async Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return StatusCodeReturn<CreateUserResponse>
                    ._403_Forbidden("User with this email already exists");
            }
            existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUser != null)
            {
                return StatusCodeReturn<CreateUserResponse>
                    ._403_Forbidden("User with this user name already exists");
            }
            var user = await CheckPoliciesAndCreateUserAsync(registerDto);
            if(user.ResponseObject != null && user.IsSuccess)
            {
                var result = await _userManager.CreateAsync(user.ResponseObject, registerDto.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user.ResponseObject);
                    var Object = new CreateUserResponse
                    {
                        Token = token,
                        User = user.ResponseObject
                    };
                    return StatusCodeReturn<CreateUserResponse>
                        ._201_Created("User created successfully", Object);
                }
                return StatusCodeReturn<CreateUserResponse>
                    ._500_ServerError("Failed to create user");
            }
            return StatusCodeReturn<CreateUserResponse>
                    ._404_NotFound(user.Message);
        }

        public async Task<ApiResponse<LoginOtpResponse>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(loginDto.UserNameOrEmail);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return StatusCodeReturn<LoginOtpResponse>
                        ._400_BadRequest("Please confirm your email");
                }
                await _signInManager.SignOutAsync();
                if (user.TwoFactorEnabled)
                {
                    var signIn = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                    if (!signIn.Succeeded)
                    {
                        return StatusCodeReturn<LoginOtpResponse>
                            ._400_BadRequest("Invalid user name or password");
                    }
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    var ResponseObject = new LoginOtpResponse
                    {
                        IsTwoFactorEnabled = user.TwoFactorEnabled,
                        Token = token,
                        User = user
                    };
                    return StatusCodeReturn<LoginOtpResponse>
                        ._200_Success("OTP generated successfully", ResponseObject);
                }
                else
                {
                    var signIn = await _signInManager.PasswordSignInAsync(user,
                            loginDto.Password, false, false);
                    if (!signIn.Succeeded)
                    {
                        return StatusCodeReturn<LoginOtpResponse>
                            ._400_BadRequest("Invalid user name or password");
                    }
                    var ResponseObject = new LoginOtpResponse
                    {
                        IsTwoFactorEnabled = user.TwoFactorEnabled,
                        Token = new JwtSecurityTokenHandler().WriteToken(
                            await _tokenService.GenerateUserToken(user))
                    };
                    return StatusCodeReturn<LoginOtpResponse>
                        ._200_Success("Token generated successfully", ResponseObject);
                }

            }
            return StatusCodeReturn<LoginOtpResponse>
                ._404_NotFound("User not found");
        }

        public async Task<ApiResponse<LoginResponse>> LoginUserWithOTPAsync(string otp, 
            string userNameOrEmail)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userNameOrEmail);
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", otp, false, false);
            if (signIn.Succeeded)
            {
                if (user != null)
                {
                    return await _tokenService.GetJwtTokenAsync(user);
                }
            }
            return StatusCodeReturn<LoginResponse>
                ._400_BadRequest("Invalid OTP");
        }
        private async Task<ApiResponse<SiteUser>> CheckPoliciesAndCreateUserAsync(RegisterDto registerDto)
        {
            var policy = await _policyService.GetPolicyByNameAsync("public");
            if (policy != null && policy.ResponseObject != null)
            {
                var user = new SiteUser
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                    DisplayName = registerDto.DisplayName,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    AccountPolicyId = policy.ResponseObject.Id,
                    AccountPostPolicyId = policy.ResponseObject.Id,
                    ReactPolicyId = policy.ResponseObject.Id,
                    CommentPolicyId = policy.ResponseObject.Id,
                    FriendListPolicyId = policy.ResponseObject.Id
                };
                return StatusCodeReturn<SiteUser>
                    ._200_Success("User generated successfully", user);
            }
            return StatusCodeReturn<SiteUser>
                            ._404_NotFound("Policy not found");
        }
    }
}