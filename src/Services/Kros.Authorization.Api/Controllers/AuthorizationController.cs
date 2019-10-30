using Kros.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.OAuthSchemeName)]
    public class AuthorizationController : ControllerBase
    {
        private readonly Application.Services.IAuthorizationService _authorizationService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">Authorization service.</param>
        public AuthorizationController(Application.Services.IAuthorizationService authorizationService)
            => _authorizationService = authorizationService;

        /// <summary>
        /// Get Jwt token with user claims.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns></returns>
        [HttpGet("jwt-token/organizations/{organizationId?}/{*other}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> GetJwtToken(long organizationId, string other)
         => await _authorizationService.CreateJwtTokenAsync(organizationId);

        /// <summary>
        /// Get Jwt token with user claims.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns></returns>
        [HttpGet("jwt-token/{*other}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> GetJwtTokenOther(string other)
         => await _authorizationService.CreateJwtTokenAsync(default);
    }
}
