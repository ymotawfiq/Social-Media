using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Service.AccountService.TwoFactoAuthenticationService
{
    public interface ITwoFactoAuthenticationService
    {
        Task<ApiResponse<string>> EnableTwoFactorAuthenticationAsync(string email);
        Task<ApiResponse<string>> DisableTwoFactorAuthenticationAsync(string email);
    }
}