
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Controllers
{
    
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [HttpGet("404")]
        public IActionResult NotFoundAsync()
        {
            return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>._404_NotFound());   
        }

        [HttpGet("401")]
        public IActionResult UnauthorizedAsync()
        {
            return StatusCode(StatusCodes.Status401Unauthorized, StatusCodeReturn<string>._401_UnAuthorized());
        }

        [HttpGet("403")]
        public IActionResult ForbiddenAsync()
        {
            return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>._403_Forbidden());
        }


    }
}
