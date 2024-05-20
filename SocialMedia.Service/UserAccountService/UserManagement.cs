

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.DTOs.Authentication.EmailConfirmation;
using SocialMedia.Data.DTOs.Authentication.Login;
using SocialMedia.Data.DTOs.Authentication.Register;
using SocialMedia.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Data.DTOs.Authentication.User;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.AccountPolicyRepository;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Service.AccountPolicyService;
using SocialMedia.Service.FriendListPolicyService;
using SocialMedia.Service.GenericReturn;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Service.UserAccountService
{
    public class UserManagement : IUserManagement
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<SiteUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IAccountPolicyService _accountPolicyService;
        private readonly IPolicyRepository _policyRepository;
        private readonly IPostRepository _postRepository;
        private readonly IReactPolicyRepository _reactPolicyRepository;
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        private readonly IFriendsRepository _friendsRepository;
        private readonly IFriendListPolicyService _friendListPolicyService;
        public UserManagement
            (
            UserManager<SiteUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IConfiguration _configuration,
            SignInManager<SiteUser> _signInManager,
            ApplicationDbContext _dbContext,
            IAccountPolicyService _accountPolicyService,
            IPolicyRepository _policyRepository,
            IPostRepository _postRepository,
            IReactPolicyRepository _reactPolicyRepository,
            ICommentPolicyRepository _commentPolicyRepository,
            IFriendsRepository _friendsRepository,
            IFriendListPolicyService _friendListPolicyService
            )
        {
            this._configuration = _configuration;
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._dbContext = _dbContext;
            this._accountPolicyService = _accountPolicyService;
            this._policyRepository = _policyRepository;
            this._postRepository = _postRepository;
            this._reactPolicyRepository = _reactPolicyRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
            this._friendsRepository = _friendsRepository;
            this._friendListPolicyService = _friendListPolicyService;
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
            var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
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
            //var accountPolicy = await _accountPolicyService.GetAccountPolicyByPolicyAsync("public");
            var user = await CheckAccountPolicyAndCreateUserAsync(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await SetAccountSettingsAsync(user.Id);
                var Object = new CreateUserResponse
                {
                    Token = token,
                    User = user
                };
                return StatusCodeReturn<CreateUserResponse>
                    ._201_Created("User created successfully", Object);
            }
            return StatusCodeReturn<CreateUserResponse>
                ._500_ServerError("Failed to create user");
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
            var user = await GetUserByUserNameOrEmailAsync(loginDto.UserNameOrEmail);
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
            var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
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
            var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
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
            var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
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


        public async Task<ApiResponse<bool>> UpdateAccountPolicyToPrivateAsync(SiteUser siteUser, 
            UpdateUserPolicyDto updateUserPolicyDto)
        {
            try
            {
                await _postRepository.UpdateUserPostsPolicyToFriendsOnlyAsync(siteUser);
                var accountPolicy = await _accountPolicyService.GetAccountPolicyByPolicyAsync(
                    updateUserPolicyDto.PolicyIdOrName);
                if (accountPolicy.ResponseObject != null)
                {
                    siteUser.AccountPolicyId = accountPolicy.ResponseObject.Id;
                    await _userManager.UpdateAsync(siteUser);
                    return StatusCodeReturn<bool>
                        ._200_Success("Accout policy updated successfully");
                }
                return StatusCodeReturn<bool>
                    ._404_NotFound("Accout policy not found");
            }
            

            catch (Exception)
            {
                throw;
            }
        }

        #region Private Method

        private async Task<SiteUser> CheckAccountPolicyAndCreateUserAsync(RegisterDto registerDto)
        {
            var accountPolicy = await _accountPolicyService.GetAccountPolicyByPolicyAsync("public");
            var user = new SiteUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString(),
                IsFriendListPrivate = true,
                AccountPolicyId = accountPolicy.ResponseObject!.Id
            };
            return user;
        }

        private async Task<SiteUser> GetUserByUserNameOrEmailAsync(string userNameOrEmail)
        {
            var userByEmail = await _userManager.FindByEmailAsync(userNameOrEmail);
            var userByName = await _userManager.FindByNameAsync(userNameOrEmail);
            if(userByEmail==null && userByName != null)
            {
                return userByName;
            }
            else if (userByName == null && userByEmail != null)
            {
                return userByEmail;
            }
            return null;
        }

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


        private async Task SetAccountSettingsAsync(string userId)
        {
            var userFriendList = await _friendsRepository.GetAllUserFriendsAsync(userId);
            if (userFriendList == null || userFriendList.ToList().Count == 0)
            {
                await _friendListPolicyService.AddFriendListPolicyAsync(
                    new AddFriendListPolicyDto
                    {
                        PolicyIdOrName = "PUBLIC",
                        UserIdOrNameOrEmail = userId
                    }
                    );
            }
        }



        #endregion

    }
}
