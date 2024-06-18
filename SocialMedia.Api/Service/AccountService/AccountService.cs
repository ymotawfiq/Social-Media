

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Api.Data.DTOs.Authentication.EmailConfirmation;
using SocialMedia.Api.Data.DTOs.Authentication.Login;
using SocialMedia.Api.Data.DTOs.Authentication.Register;
using SocialMedia.Api.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Api.Service.AccountService
{
    public class AccountService : IAccountService
    {

        private readonly Policies policies = new();

        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<SiteUser> _signInManager;
        private readonly IPostRepository _postRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IPolicyRepository _policyRepository;
        private readonly IPolicyService _policyService;
        public AccountService
            (
            UserManager<SiteUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IConfiguration _configuration,
            SignInManager<SiteUser> _signInManager,
            IPostRepository _postRepository,
            UserManagerReturn _userManagerReturn,
            IPolicyRepository _policyRepository,
            IPolicyService _policyService
            )
        {
            this._configuration = _configuration;
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._postRepository = _postRepository;
            this._userManagerReturn = _userManagerReturn;
            this._policyRepository = _policyRepository;
            this._policyService = _policyService;
        }
        public async Task<ApiResponse<List<string>>> AssignRolesToUserAsync(List<string> roles, SiteUser user)
        {
            List<string> assignRoles = new();
            var siteRoles = await _roleManager.Roles.ToListAsync();
            if (roles.Contains("Admin"))
            {
                foreach(var role in siteRoles)
                {
                    if (role.Name != null)
                    {
                        if (!await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                            assignRoles.Add(role.Name);
                        }
                    }
                }
            }
            else
            {
                foreach (var role in roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        if (!await _userManager.IsInRoleAsync(user, role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                            assignRoles.Add(role);
                        }
                    }
                }
            }
            return StatusCodeReturn<List<string>>
                ._200_Success("Roles assugned successfully to user", assignRoles);
        }

        public async Task<ApiResponse<string>> ConfirmEmail(string userNameOrEmail, string token)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userNameOrEmail);
            if (user != null)
            {
                var confirmEmail = await _userManager.ConfirmEmailAsync(user, token);
                if (!confirmEmail.Succeeded)
                {
                    return StatusCodeReturn<string>
                        ._400_BadRequest("Failed to confirm email");
                }

                return StatusCodeReturn<string>
                    ._200_Success("Email confirmed successfully");
                
            }

            return StatusCodeReturn<string>
                ._404_NotFound("User not found");
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

        public async Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(
            ResetEmailObjectDto resetEmailObjectDto)
        {
            var user = await _userManager.FindByEmailAsync(resetEmailObjectDto.OldEmail);
            if (user == null)
            {
                return StatusCodeReturn<ResetEmailDto>
                    ._404_NotFound("User not found");
            }
            else if(resetEmailObjectDto.OldEmail == resetEmailObjectDto.NewEmail)
            {
                return StatusCodeReturn<ResetEmailDto>
                    ._400_BadRequest("New email can't be same as old email");
            }
            var userWithNewEmail = await _userManager.FindByEmailAsync(resetEmailObjectDto.NewEmail);
            if (userWithNewEmail != null)
            {
                return StatusCodeReturn<ResetEmailDto>
                    ._403_Forbidden("Email already exists to user");
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, resetEmailObjectDto.NewEmail);
            var ResponseObject = new ResetEmailDto
            {
                NewEmail = resetEmailObjectDto.NewEmail,
                OldEmail = resetEmailObjectDto.OldEmail,
                Token = token
            };
            return StatusCodeReturn<ResetEmailDto>
                ._201_Created("Reset email token generated successfully", ResponseObject);
        }

        public async Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return StatusCodeReturn<ResetPasswordDto>
                    ._404_NotFound("User not found");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var ResponseObject = new ResetPasswordDto
            {
                Token = token,
                Email = email
            };
            return StatusCodeReturn<ResetPasswordDto>
                ._200_Success("Reset password token generated successfully", ResponseObject);
        }

        public async Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(SiteUser user)
        {
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenValidity);
            await _userManager.UpdateAsync(user);
            var ResponseObject = new LoginResponse
            {
                AccessToken = new TokenType
                {
                    ExpiryTokenDate = (await GenerateUserToken(user)).ValidTo,
                    Token = new JwtSecurityTokenHandler().WriteToken(await GenerateUserToken(user))
                },
                RefreshToken = new TokenType
                {
                    Token = refreshToken,
                    ExpiryTokenDate = (DateTime)user.RefreshTokenExpiry
                }
            };
            return StatusCodeReturn<LoginResponse>
                ._200_Success("Token created successfully", ResponseObject);
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
                        Token = new JwtSecurityTokenHandler().WriteToken(await GenerateUserToken(user))
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
                    return await GetJwtTokenAsync(user);
                }
            }
            return StatusCodeReturn<LoginResponse>
                ._400_BadRequest("Invalid OTP");
        }

        public async Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens)
        {
            var accessToken = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;
            if(accessToken==null || refreshToken == null)
            {
                return StatusCodeReturn<LoginResponse>
                    ._400_BadRequest("Access token or refresh token must not be null");
            }
            var principal = GetClaimsPrincipal(accessToken.Token);
            if(principal.Identity==null || principal.Identity.Name == null)
            {
                return StatusCodeReturn<LoginResponse>
                    ._400_BadRequest("Principal name empty");
            }
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null)
            {
                return StatusCodeReturn<LoginResponse>
                    ._404_NotFound("User not found");
            }

            return await GetJwtTokenAsync(user);
        }

        public async Task<ApiResponse<EmailConfirmationDto>> GenerateEmailConfirmationTokenAsync
            (string userNameOrEmail)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userNameOrEmail);
            if(user == null)
            {
                return StatusCodeReturn<EmailConfirmationDto>
                    ._404_NotFound("User not found");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var ResponseObject = new EmailConfirmationDto
            {
                Token = token,
                User = user
            };
            return StatusCodeReturn<EmailConfirmationDto>
                ._200_Success("Email confirmation token generated successfully", ResponseObject);
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return StatusCodeReturn<string>
                    ._404_NotFound("User not found");
            }
            var result = await _userManager.ResetPasswordAsync(user,
                resetPasswordDto.Token, resetPasswordDto.Password);
            if (result.Succeeded)
            {
                return StatusCodeReturn<string>
                    ._200_Success("Password reset successfully");
            }
            return StatusCodeReturn<string>
                ._400_BadRequest("Failed to reset password");
        }

        public async Task<ApiResponse<string>> EnableTwoFactorAuthenticationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCodeReturn<string>
                    ._404_NotFound("User not found");
            }
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            await _userManager.UpdateAsync(user);
            return StatusCodeReturn<string>
                ._200_Success("Two factor authentication enabled successfully");
        }

        public async Task<ApiResponse<string>> DeleteAccountAsync(string userNameOrEmail)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userNameOrEmail);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "Account not found",
                    StatusCode = 404
                };
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "Failed to delete Account",
                    StatusCode = 400
                };
            }
            return new ApiResponse<string>
            {
                StatusCode = 200,
                Message = "Account deleted successfully",
                IsSuccess = true
            };
        }


        public async Task<ApiResponse<bool>> UpdateAccountCommentPolicyAsync(SiteUser user, 
            string policyIdOrName)
        {
            var commentPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (commentPolicy != null && commentPolicy.ResponseObject != null)
            {
                if (policies.CommentPolicies.Contains(commentPolicy.ResponseObject.PolicyType))
                {
                    user.CommentPolicyId = commentPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont comment policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> UpdateAccountReactPolicyAsync(SiteUser user,
            string policyIdOrName)
        {
            var reactPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (reactPolicy != null && reactPolicy.ResponseObject != null)
            {
                if (policies.ReactPolicies.Contains(reactPolicy.ResponseObject.PolicyType))
                {
                    user.ReactPolicyId = reactPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont react policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> UpdateAccountPostsPolicyAsync(SiteUser user,
            string policyIdOrName)
        {
            var accountPostsPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (accountPostsPolicy != null && accountPostsPolicy.ResponseObject != null)
            {
                if (policies.AccountPostPolicies.Contains(accountPostsPolicy.ResponseObject.PolicyType))
                {
                    user.AccountPostPolicyId = accountPostsPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont posts policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                     ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> UpdateAccountFriendListPolicyAsync(SiteUser user,
            string policyIdOrName)
        {
            var accountFriendListPolicy = await _policyService.GetPolicyByIdOrNameAsync(policyIdOrName);
            if (accountFriendListPolicy != null && accountFriendListPolicy.ResponseObject != null)
            {
                if (policies.FriendListPolicies.Contains(accountFriendListPolicy.ResponseObject.PolicyType))
                {
                    user.FriendListPolicyId = accountFriendListPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(user);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accont friend list policy updated successfully", true);
                }
                return StatusCodeReturn<bool>
                     ._403_Forbidden("Invalid policy");
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Policy not found");
        }

        public async Task<ApiResponse<bool>> LockProfileAsync(SiteUser user)
        {
            var privatePolicy = await _policyRepository.GetPolicyByNameAsync("private");
            var friendsPolicy = await _policyRepository.GetPolicyByNameAsync("friends only");
            if (privatePolicy != null)
            {
                if(user.AccountPolicyId != privatePolicy.Id)
                {
                    if (friendsPolicy != null)
                    {
                        user.ReactPolicyId = user.CommentPolicyId = user.AccountPostPolicyId
                            = user.FriendListPolicyId = friendsPolicy.Id;
                        user.AccountPolicyId = privatePolicy.Id;
                        await _userManager.UpdateAsync(user);
                        await _postRepository.UpdateUserPostsPolicyToLockedAccountAsync(user.Id);
                        return StatusCodeReturn<bool>
                            ._200_Success("Profile locked successfully", true);
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("Friends only policy not found", false);
                }
                return StatusCodeReturn<bool>
                        ._403_Forbidden("Account already locked");
            }
            return StatusCodeReturn<bool>
                        ._404_NotFound("Private policy not found", false);
        }

        public async Task<ApiResponse<bool>> UnLockProfileAsync(SiteUser user)
        {
            var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
            if (publicPolicy != null)
            {
                if(user.AccountPolicyId != publicPolicy.Id)
                {
                    user.ReactPolicyId = user.CommentPolicyId = user.AccountPostPolicyId
                    = user.FriendListPolicyId = user.AccountPolicyId = publicPolicy.Id;
                    await _userManager.UpdateAsync(user);
                    await _postRepository.UpdateUserPostsPolicyToUnLockedAccountAsync(user.Id);
                    return StatusCodeReturn<bool>
                        ._200_Success("Profile unlocked successfully", true);
                }
                return StatusCodeReturn<bool>
                        ._403_Forbidden("Account already public");
            }
            return StatusCodeReturn<bool>
                        ._404_NotFound("Public policy not found", false);
        }


        #region Private Method

        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<JwtSecurityToken> GenerateUserToken(SiteUser siteUser)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, siteUser.Email),
                new Claim(ClaimTypes.Name, siteUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userRoles = await _userManager.GetRolesAsync(siteUser);
            foreach(var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var jwtToken = GetToken(authClaims);
            return jwtToken;
        }

        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var expirationTime = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTime, localTimeZone);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationTimeInLocalTimeZone,
                claims:claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters,
                out SecurityToken securityToken);
            return principal;
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




        #endregion

    }
}
