

using Microsoft.AspNetCore.Identity;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.GenericReturn
{
    public class UserManagerReturn
    {

        private readonly UserManager<SiteUser> _userManager;
        public UserManagerReturn(UserManager<SiteUser> _userManager)
        {
            this._userManager = _userManager;
        }
        public UserManagerReturn()
        {
            
        }
        public async Task<SiteUser> GetUserByUserNameOrEmailOrIdAsync(string userNameOrEmailOrId)
        {
            var userById = await _userManager.FindByIdAsync(userNameOrEmailOrId);
            var userByEmail = await _userManager.FindByEmailAsync(userNameOrEmailOrId);
            var userByName = await _userManager.FindByNameAsync(userNameOrEmailOrId);
            if (userByName != null)
            {
                return userByName;
            }
            else if (userByEmail != null)
            {
                return userByEmail;
            }
            else if (userById != null)
            {
                return userById;
            }
            return null!;
        }
    }
}
