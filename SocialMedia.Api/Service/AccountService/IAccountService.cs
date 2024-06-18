

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.DTOs.Authentication.EmailConfirmation;
using SocialMedia.Api.Data.DTOs.Authentication.Login;
using SocialMedia.Api.Data.DTOs.Authentication.Register;
using SocialMedia.Api.Data.DTOs.Authentication.ResetEmail;
using SocialMedia.Api.Data.DTOs.Authentication.ResetPassword;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.AccountService
{
    public interface IAccountService
    {
        Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterDto registerDto);
        Task<ApiResponse<List<string>>> AssignRolesToUserAsync(List<string> roles, SiteUser user);
        Task<ApiResponse<LoginOtpResponse>> LoginUserAsync(LoginDto loginDto);
        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(SiteUser user);
        Task<ApiResponse<LoginResponse>> LoginUserWithOTPAsync(string otp, string userNameOrEmail);
        Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens);
        Task<ApiResponse<string>> ConfirmEmail(string userNameOrEmail, string token);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email);
        Task<ApiResponse<EmailConfirmationDto>> GenerateEmailConfirmationTokenAsync(string email);
        Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(ResetEmailObjectDto resetEmailObjectDto);
        Task<ApiResponse<string>> EnableTwoFactorAuthenticationAsync(string email);
        Task<ApiResponse<string>> DeleteAccountAsync(string userNameOrEmail);
        Task<ApiResponse<bool>> UpdateAccountCommentPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<bool>> UpdateAccountReactPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<bool>> UpdateAccountFriendListPolicyAsync(SiteUser user, string policyIdOrName);
        Task<ApiResponse<bool>> LockProfileAsync(SiteUser user);
        Task<ApiResponse<bool>> UnLockProfileAsync(SiteUser user);
        Task<ApiResponse<bool>> UpdateAccountPostsPolicyAsync(SiteUser user, string policyIdOrName);
    }
}
