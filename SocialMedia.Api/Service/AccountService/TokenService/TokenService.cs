using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Api.Data.DTOs.Authentication.EmailConfirmation;
using SocialMedia.Api.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.AccountService.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly UserManagerReturn _userManagerReturn;
        public TokenService(UserManager<SiteUser> _userManager, IConfiguration _configuration, 
            UserManagerReturn _userManagerReturn)
        {
            this._userManager  = _userManager;
            this._configuration = _configuration;
            this._userManagerReturn = _userManagerReturn;
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
        public string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<JwtSecurityToken> GenerateUserToken(SiteUser siteUser)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, siteUser.Email!),
                new Claim(ClaimTypes.Name, siteUser.UserName!),
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
        public JwtSecurityToken GetToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
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
                    Token = new JwtSecurityTokenHandler().WriteToken(
                        await GenerateUserToken(user))
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

        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!))
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters,
                out SecurityToken securityToken);
            return principal;
        }
    }
}