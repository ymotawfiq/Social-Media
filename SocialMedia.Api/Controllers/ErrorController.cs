
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [HttpGet("404")]
        public async Task<IActionResult> NotFoundAsync()
        {
            return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>._404_NotFound());   
        }

        [HttpGet("401")]
        public async Task<IActionResult> UnauthorizedAsync()
        {
            return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>._401_UnAuthorized());
        }

        [HttpGet("403")]
        public async Task<IActionResult> ForbiddenAsync()
        {
            return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>._403_Forbidden());
        }


    }
}
