using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.AccountService.RolesService
{
    public class RolesService : IRolesService
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesService(UserManager<SiteUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }
        public async Task<ApiResponse<List<string>>> AssignRolesToUserAsync(List<string> roles, SiteUser user)
        {
            roles = NormalizeRoles(roles);
            List<string> assignRoles = new();
            var siteRoles = await _roleManager.Roles.ToListAsync();
            if (roles.Contains("ADMIN"))
            {
                foreach(var role in siteRoles)
                {
                    if (role.Name != null)
                    {
                        if (!await _userManager.IsInRoleAsync(user, role.Name.ToUpper()))
                        {
                            await _userManager.AddToRoleAsync(user, role.Name.ToUpper());
                            assignRoles.Add(role.Name.ToUpper());
                        }
                    }
                }
            }
            else
            {
                foreach (var role in roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        if (!await _userManager.IsInRoleAsync(user, role))
                        {
                            await _userManager.AddToRoleAsync(user, role);
                            assignRoles.Add(role);
                        }
                    }
                }
            }
            return StatusCodeReturn<List<string>>
                ._200_Success("Roles assugned successfully to user", assignRoles);
        }
        
                private List<string> NormalizeRoles(List<string> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                return new List<string> { "USER" };
            }
            for (int i = 0; i < roles.Count; i++)
            {
                roles[i] = roles.ElementAt(i).ToUpper();
            }
            return roles;
        }
    }
}