

using Microsoft.AspNetCore.Identity;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.GenericReturn
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

        public SiteUser SetUserToReturn(SiteUser user)
        {
            if (user != null)
            {
                return new SiteUser
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DisplayName = user.DisplayName
                };
            }
            return null!;
        }


    }
}
