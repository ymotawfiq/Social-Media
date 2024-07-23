using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.AccountService.RolesService
{
    public interface IRolesService
    {
        Task<ApiResponse<List<string>>> AssignRolesToUserAsync(List<string> roles, SiteUser user);
    }
}