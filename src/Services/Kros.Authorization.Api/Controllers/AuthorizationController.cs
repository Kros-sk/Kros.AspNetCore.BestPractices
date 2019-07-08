using Kros.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("jwt-token")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<string> GetJwtToken()
         => await _authorizationService.CreateJwtTokenAsync();
    }
}
