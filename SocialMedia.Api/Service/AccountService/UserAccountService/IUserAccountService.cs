using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Api.Data.DTOs.Authentication.Login;
using SocialMedia.Api.Data.DTOs.Authentication.Register;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Service.AccountService.UserAccountService
{
    public interface IUserAccountService
    {
        Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterDto registerDto);
        Task<ApiResponse<LoginOtpResponse>> LoginUserAsync(LoginDto loginDto);
        Task<ApiResponse<LoginResponse>> LoginUserWithOTPAsync(string otp, string userNameOrEmail);
    }
}