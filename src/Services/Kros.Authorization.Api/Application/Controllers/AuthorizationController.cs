using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.OAuthSchemeName)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class AuthorizationController : ApiBaseController
    {
        private readonly Application.Services.IAuthorizationService _authorizationService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">Authorization service.</param>
        public AuthorizationController(Application.Services.IAuthorizationService authorizationService)
            => _authorizationService = Check.NotNull(authorizationService, nameof(authorizationService));

        /// <summary>
        /// Get Jwt token with user claims.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns>JWT token.</returns>
        [HttpGet("jwt-token/organizations/{organizationId?}/{*other}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> GetJwtToken(long organizationId, string other)
         => await _authorizationService.CreateJwtTokenAsync(organizationId);

        /// <summary>
        /// Get Jwt token with user claims.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns>JWT token.</returns>
        [HttpGet("jwt-token/{*other}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<string> GetJwtTokenOther(string other)
         => await _authorizationService.CreateJwtTokenAsync(default);
    }
}
