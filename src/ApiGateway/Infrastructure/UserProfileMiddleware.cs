using Flurl;
using IdentityModel;
using IdentityModel.Client;
using Kros.Identity.Extensions;
using Kros.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiGateway.Infrastructure
{
    /// <summary>
    /// Middleware for user profile.
    /// </summary>
    public class UserProfileMiddleware
    {
        /// <summary>
        /// Http client name for communication with Identity Server.
        /// </summary>
        public const string IdentityServerHttpClientName = "IdentityServerClient";
        private const string IdentityServerUserInfoEndpoint = "connect/userinfo";

        private readonly RequestDelegate _next;
        private readonly IdentityServerOptions _identityServerOptions;
        private IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        /// <param name="cache">Cache service.</param>
        /// <param name="identityServerOptions">Identity Server options.</param>
        public UserProfileMiddleware(
            RequestDelegate next,
            IdentityServerOptions identityServerOptions)
        {
            _next = Check.NotNull(next, nameof(next));
            _identityServerOptions = identityServerOptions;
        }

        /// <summary>
        /// HttpContext pipeline processing.
        /// </summary>
        /// <param name="httpContext">Http context.</param>
        /// <param name="httpClientFactory">Http client factory.</param>
        public async Task Invoke(
            HttpContext httpContext,
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            var userClaims = await GetUserProfileClaimsAsync(httpContext);
            if (userClaims != null)
            {
                AddUserProfileClaimsToCurrentIdentity(userClaims, httpContext);
            }
            

            await _next(httpContext);
        }

        /// <summary>
        /// Get user profile's claims from IdentityServer user profile endpoint.
        /// </summary>
        /// <param name="httpContext">Current Http context.</param>
        /// <returns>User profile's claims.</returns>
        private async Task<IEnumerable<Claim>> GetUserProfileClaimsAsync(HttpContext httpContext)
        {
            string token = await httpContext.GetTokenAsync(
                    OidcConstants.AuthenticationSchemes.FormPostBearer); // FormPostBearer == "access_token"

            if (token != null)
            {
                using (var client = _httpClientFactory.CreateClient(IdentityServerHttpClientName))
                {
                    var response = await client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = Url.Combine(_identityServerOptions.AuthorityUrl, IdentityServerUserInfoEndpoint),
                        Token = token
                    });

                    if (!response.IsError)
                    {
                        return response.Claims;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Add user profile's claims to the current user identity.
        /// </summary>
        /// <param name="userClaims">User's claims.</param>
        /// <param name="httpContext">Current Http context.</param>
        private void AddUserProfileClaimsToCurrentIdentity(
            IEnumerable<Claim> userClaims,
            HttpContext httpContext)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            if (httpContext.User?.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject) == null)
            {
                Claim subClaim = userClaims?.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
                if (subClaim != null)
                {
                    claimsIdentity.AddClaim(subClaim);
                }
            }

            Claim emailClaim = userClaims?.FirstOrDefault(x => x.Type == JwtClaimTypes.Email);
            if (emailClaim != null)
            {
                claimsIdentity.AddClaim(emailClaim);
            }

            httpContext.User?.AddIdentity(claimsIdentity);
        }
    }
}