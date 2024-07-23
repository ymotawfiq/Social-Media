using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.AccountService.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly UserManagerReturn _userManagerReturn;
        private readonly UserManager<SiteUser> _userManager;
        public EmailService(UserManagerReturn _userManagerReturn, UserManager<SiteUser> _userManager)
        {
            this._userManagerReturn = _userManagerReturn;
            this._userManager = _userManager;
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
    }
}