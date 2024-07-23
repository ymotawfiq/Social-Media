using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Service.AccountService.EmailService
{
    public interface IEmailService
    {
        Task<ApiResponse<string>> ConfirmEmail(string userNameOrEmail, string token);
    }
}