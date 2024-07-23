
using Microsoft.AspNetCore.Identity;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.AccountService.TwoFactoAuthenticationService
{
    public class TwoFactoAuthenticationService : ITwoFactoAuthenticationService
    {
        private readonly UserManager<SiteUser> _userManager;
        public TwoFactoAuthenticationService(UserManager<SiteUser> _userManager)
        {
            this._userManager = _userManager;
        }
        public async Task<ApiResponse<string>> EnableTwoFactorAuthenticationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCodeReturn<string>
                    ._404_NotFound("User not found");
            }
            if(user.TwoFactorEnabled){
                return StatusCodeReturn<string>
                    ._403_Forbidden("Two factor authentication active");    
            }
            await _userManager.SetTwoFactorEnabledAsync(user, true);
            await _userManager.UpdateAsync(user);
            return StatusCodeReturn<string>
                ._200_Success("Two factor authentication enabled successfully");
        }

        public async Task<ApiResponse<string>> DisableTwoFactorAuthenticationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return StatusCodeReturn<string>
                    ._404_NotFound("User not found");
            }
            if(!user.TwoFactorEnabled){
                return StatusCodeReturn<string>
                    ._403_Forbidden("Two factor authentication not active");    
            }
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.UpdateAsync(user);
            return StatusCodeReturn<string>
                ._200_Success("Two factor authentication enabled successfully");
        }




    }
}