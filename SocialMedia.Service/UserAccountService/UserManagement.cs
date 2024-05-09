

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using SocialMedia.Data;
using SocialMedia.Data.DTOs.Authentication.EmailConfirmation;
using SocialMedia.Data.DTOs.Authentication.Login;
using SocialMedia.Data.DTOs.Authentication.Register;
using SocialMedia.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Data.DTOs.Authentication.User;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.AccountPolicyRepository;
using SocialMedia.Service.AccountPolicyService;
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
        public UserManagement
            (
            UserManager<SiteUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IConfiguration _configuration,
            SignInManager<SiteUser> _signInManager,
            ApplicationDbContext _dbContext,
            IAccountPolicyService _accountPolicyService
            )
        {
            this._configuration = _configuration;
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._dbContext = _dbContext;
            this._accountPolicyService = _accountPolicyService;
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
            return new ApiResponse<List<string>>
            {
                IsSuccess = true,
                Message = "Roles assugned successfully to user",
                StatusCode = 200,
                ResponseObject = assignRoles
            };
        }

        public async Task<ApiResponse<string>> ConfirmEmail(string userNameOrEmail, string token)
        {
            var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
            if (user != null)
            {
                var confirmEmail = await _userManager.ConfirmEmailAsync(user, token);
                if (!confirmEmail.Succeeded)
                {
                    return new ApiResponse<string>
                    {
                        IsSuccess = true,
                        Message = "Failed to confirm email",
                        StatusCode = 400,
                        ResponseObject = "Failed to confirm email"
                    };
                }

                return new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "Email confirmed successfully",
                    StatusCode = 200,
                    ResponseObject = "Email confirmed successfully"
                };
                
            }

            return new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "User not found",
                StatusCode = 404,
                ResponseObject = "User not found"
            };
        }

        public async Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new ApiResponse<CreateUserResponse>
                {
                    IsSuccess = false,
                    Message = "User with this email already exists",
                    StatusCode = 403
                };
            }
            existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUser != null)
            {
                return new ApiResponse<CreateUserResponse>
                {
                    IsSuccess = false,
                    Message = "User with this user name already exists",
                    StatusCode = 403
                };
            }
            //var accountPolicy = await _accountPolicyService.GetAccountPolicyByPolicyAsync("public");
            var user = await CheckAccountPolicyAndCreateUserAsync(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return new ApiResponse<CreateUserResponse>
                {
                    IsSuccess = true,
                    Message = "User created successfully",
                    StatusCode = 201,
                    ResponseObject = new CreateUserResponse
                    {
                        Token = token,
                        User = user
                    }
                };
            }
            return new ApiResponse<CreateUserResponse>
            {
                IsSuccess = false,
                Message = "Failed to create user",
                StatusCode = 500
            };
        }

        public async Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(
            ResetEmailObjectDto resetEmailObjectDto)
        {
            var user = await _userManager.FindByEmailAsync(resetEmailObjectDto.OldEmail);
            if (user == null)
            {
                return new ApiResponse<ResetEmailDto>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            else if(resetEmailObjectDto.OldEmail == resetEmailObjectDto.NewEmail)
            {
                return new ApiResponse<ResetEmailDto>
                {
                    IsSuccess = false,
                    Message = "New email can't be same as old email",
                    StatusCode = 400  
                };
            }
            var userWithNewEmail = await _userManager.FindByEmailAsync(resetEmailObjectDto.NewEmail);
            if (userWithNewEmail != null)
            {
                return new ApiResponse<ResetEmailDto>
                {
                    IsSuccess = false,
                    Message = "Email already exists to user",
                    StatusCode = 403
                };
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, resetEmailObjectDto.NewEmail);
            return new ApiResponse<ResetEmailDto>
            {
                IsSuccess = true,
                Message = "Reset email token generated successfully",
                StatusCode = 200,
                ResponseObject = new ResetEmailDto
                {
                    NewEmail = resetEmailObjectDto.NewEmail,
                    OldEmail = resetEmailObjectDto.OldEmail,
                    Token = token
                }
            };
        }

        public async Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return new ApiResponse<ResetPasswordDto>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new ApiResponse<ResetPasswordDto>
            {
                StatusCode = 200,
                Message = "Reset password token generated successfully",
                IsSuccess = true,
                ResponseObject = new ResetPasswordDto
                {
                    Token = token,
                    Email = email
                }
            };
        }

        public async Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(SiteUser user)
        {
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(refreshTokenValidity);
            await _userManager.UpdateAsync(user);
            return new ApiResponse<LoginResponse>
            {
                IsSuccess = true,
                Message = "Token created successfully",
                StatusCode = 200,
                ResponseObject = new LoginResponse
                {
                    AccessToken = new TokenType
                    {
                        ExpiryTokenDate = (await GenerateUserToken(user)).ValidTo,
                        Token = new JwtSecurityTokenHandler().WriteToken(await GenerateUserToken(user))
                    },
                    RefreshToken = new TokenType
                    {
                        Token = refreshToken,
                        ExpiryTokenDate = (DateTime) user.RefreshTokenExpiry
                    }
                }
            };
        }

        public async Task<ApiResponse<LoginOtpResponse>> LoginUserAsync(LoginDto loginDto)
        {
            var user = await GetUserByUserNameOrEmailAsync(loginDto.UserNameOrEmail);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return new ApiResponse<LoginOtpResponse>
                    {
                        IsSuccess = false,
                        Message = "Please confirm your email",
                        StatusCode = 400
                    };
                }
                await _signInManager.SignOutAsync();
                if (user.TwoFactorEnabled)
                {
                    var signIn = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                    if (!signIn.Succeeded)
                    {
                        return new ApiResponse<LoginOtpResponse>
                        {
                            IsSuccess = false,
                            Message = "Invalid user name or password",
                            StatusCode = 400
                        };
                    }
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    return new ApiResponse<LoginOtpResponse>
                    {
                        IsSuccess = true,
                        Message = "OTP generated successfully",
                        StatusCode = 200,
                        ResponseObject = new LoginOtpResponse
                        {
                            IsTwoFactorEnabled = user.TwoFactorEnabled,
                            Token = token,
                            User = user
                        }
                    };
                }
                else
                {
                    var signIn = await _signInManager.PasswordSignInAsync(user,
                            loginDto.Password, false, false);
                    if (!signIn.Succeeded)
                    {
                        return new ApiResponse<LoginOtpResponse>
                        {
                            IsSuccess = false,
                            Message = "Invalid user name or password",
                            StatusCode = 400
                        };
                    }
                    return new ApiResponse<LoginOtpResponse>
                    {
                        IsSuccess = true,
                        Message = "Token generated successfully",
                        StatusCode = 200,
                        ResponseObject = new LoginOtpResponse
                        {
                            IsTwoFactorEnabled = user.TwoFactorEnabled,
                            Token = new JwtSecurityTokenHandler().WriteToken(await GenerateUserToken(user))
                        }
                    };
                }

            }
            return new ApiResponse<LoginOtpResponse>
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = "User not found"
            };
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
            return new ApiResponse<LoginResponse>
            {
                IsSuccess = false,
                StatusCode = 400,
                Message = "Invalid OTP"
            };
        }

        public async Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens)
        {
            var accessToken = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;
            if(accessToken==null || refreshToken == null)
            {
                return new ApiResponse<LoginResponse>
                {
                    IsSuccess = false,
                    Message = "Access token or refresh token must not be null",
                    StatusCode = 400
                };
            }
            var principal = GetClaimsPrincipal(accessToken.Token);
            if(principal.Identity==null || principal.Identity.Name == null)
            {
                return new ApiResponse<LoginResponse>
                {
                    IsSuccess = false,
                    Message = "Principal name empty",
                    StatusCode = 400
                };
            }
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null)
            {
                return new ApiResponse<LoginResponse>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }

            return await GetJwtTokenAsync(user);
        }

        public async Task<ApiResponse<EmailConfirmationDto>> GenerateEmailConfirmationTokenAsync
            (string userNameOrEmail)
        {
            var user = await GetUserByUserNameOrEmailAsync(userNameOrEmail);
            if(user == null)
            {
                return new ApiResponse<EmailConfirmationDto>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return new ApiResponse<EmailConfirmationDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Email confirmation token generated successfully",
                ResponseObject = new EmailConfirmationDto
                {
                   Token = token,
                    User = user
                }
            };
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var result = await _userManager.ResetPasswordAsync(user,
                resetPasswordDto.Token, resetPasswordDto.Password);
            if (result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = true,
                    Message = "Password reset successfully",
                    StatusCode = 200
                };
            }
            return new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "Failed to reset password",
                StatusCode = 400
            };
        }

        public async Task<ApiResponse<string>> EnableTwoFactorAuthenticationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            await _userManager.UpdateAsync(user);
            return new ApiResponse<string>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Two factor authentication enabled successfully"
            };
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

        #endregion

    }
}
