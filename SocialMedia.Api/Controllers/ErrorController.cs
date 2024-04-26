
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Api.Controllers
{
    
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly UserManager<SiteUser> _userManager;
        public ErrorController(UserManager<SiteUser> _userManager)
        {
            this._userManager = _userManager;
        }

        [HttpGet("404")]
        public async Task<IActionResult> NotFoundAsync()
        {
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
            {
                StatusCode = 404,
                IsSuccess = false,
                Message = "Page Not Found",
                ResponseObject = "Page Not Found"
            });   
        }

        [HttpGet("401")]
        public async Task<IActionResult> UnauthorizedAsync()
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
            {
                StatusCode = 401,
                IsSuccess = false,
                Message = "Unauthorized",
                ResponseObject = "Unauthorized"
            });
        }

        [HttpGet("403")]
        public async Task<IActionResult> ForbiddenAsync()
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
            {
                StatusCode = 403,
                IsSuccess = false,
                Message = "Forbidden",
                ResponseObject = "Forbidden"
            });
        }

        [HttpGet("login")]
        public string Login()
        {
            return "Please login";
        }

    }
}
