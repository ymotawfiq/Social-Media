using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SocialMedia.Api.Data.DTOs.Authentication.EmailConfirmation;
using SocialMedia.Api.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.AccountService.TokenService
{
    public interface ITokenService
    {
        public Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(
                ResetEmailObjectDto resetEmailObjectDto);
        public string GenerateRefreshToken();
        public Task<JwtSecurityToken> GenerateUserToken(SiteUser siteUser);
        public JwtSecurityToken GetToken(List<Claim> claims);
        public Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email);
        public Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens);
        public Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(SiteUser user);
        public Task<ApiResponse<EmailConfirmationDto>> GenerateEmailConfirmationTokenAsync(
            string userNameOrEmail);
    }
}