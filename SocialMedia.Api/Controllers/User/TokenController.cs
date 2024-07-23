using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.DTOs.Authentication.User;
using SocialMedia.Api.Service.AccountService.TokenService;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService _tokenService)
        {
            this._tokenService = _tokenService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(LoginResponse tokens)
        {
            try
            {
                var response = await _tokenService.RenewAccessTokenAsync(tokens);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    StatusCodeReturn<string>._500_ServerError(ex.Message));
            }
        }
    }
}