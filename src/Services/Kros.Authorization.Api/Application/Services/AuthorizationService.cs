using IdentityModel.Client;
using Kros.Authorization.Api.Application.Options;
using Kros.Authorization.Api.Application.Queries;
using Kros.Authorization.Api.Application.Shared;
using Kros.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Kros.Authorization.Api.Application.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JwtAuthorizationSecurityOptions _jwtAuthorizationSecurityOptions;
        private readonly IUserService _userService;
        private readonly HttpContext _httpContext;

        public AuthorizationService(
            IHttpClientFactory httpClientFactory,
            IOptions<JwtAuthorizationSecurityOptions> jwtAuthorizationSecurityOptions,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = Check.NotNull(httpClientFactory, nameof(httpClientFactory));
            _jwtAuthorizationSecurityOptions = Check.NotNull(jwtAuthorizationSecurityOptions, nameof(jwtAuthorizationSecurityOptions)).Value;
            _userService = Check.NotNull(userService, nameof(userService));
            _httpContext = Check.NotNull(httpContextAccessor, nameof(httpContextAccessor)).HttpContext;
        }

        public async Task<string> CreateJwtTokenAsync()
        {
            var oidcClaims = await GetOidcUserClaimsAsync();
            var user = await _userService.TryCreateDefaultUserAsync(oidcClaims);

            var allUserClaims = new List<Claim>();
            allUserClaims.AddRange(oidcClaims);
            allUserClaims.AddRange(GetUserAuthorizationClaims(user));

            return JwtHelper.CreateJwtTokenFromClaims(allUserClaims, _jwtAuthorizationSecurityOptions.JwtSecret);
        }

        private IEnumerable<Claim> GetUserAuthorizationClaims(GetUserByEmailQuery.User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim("IsAdmin", user.IsAdmin.ToString()));

            return claims;
        }

        private async Task<IEnumerable<Claim>> GetOidcUserClaimsAsync()
        {
            string accessToken = await _httpContext.GetTokenAsync("IS", AuthenticationSchemes.FormPostBearer);
            if (accessToken != null)
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var response = await client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = _jwtAuthorizationSecurityOptions.IdentityServerUserInfoEndpoint,
                        Token = accessToken
                    });

                    if (!response.IsError)
                    {
                        return response.Claims;
                    }
                }
            }

            return new List<Claim>();
        }
    }
}
