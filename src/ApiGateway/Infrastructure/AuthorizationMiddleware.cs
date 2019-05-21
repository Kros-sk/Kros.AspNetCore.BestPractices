using Kros.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace ApiGateway.Infrastructure
{
    /// <summary>
    /// Middleware for user authorization.
    /// </summary>
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtAuthorizationOptions _jwtAuthorizationOptions;
        private IHttpClientFactory _httpClientFactory;
        private HttpContext _httpContext;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        /// <param name="jwtAuthorizationOptions">Authorization options.</param>
        public AuthorizationMiddleware(
            RequestDelegate next,
            JwtAuthorizationOptions jwtAuthorizationOptions)
        {
            _next = Check.NotNull(next, nameof(next));
            _jwtAuthorizationOptions = Check.NotNull(jwtAuthorizationOptions, nameof(jwtAuthorizationOptions));
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
            _httpContext = httpContext;
            _httpClientFactory = httpClientFactory;

            var userJwt = await GetUserAuthorizationJwtAsync();

            if (!string.IsNullOrEmpty(userJwt))
            {
                AddUserProfileClaimsToIdentityAndHttpHeaders(userJwt);
            }

            await _next(httpContext);
        }

        private async Task<string> GetUserAuthorizationJwtAsync()
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                if (_httpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out StringValues value)) {
                    client.DefaultRequestHeaders.Add(HeaderNames.Authorization, value.ToString());

                    var response = await client.GetAsync(_jwtAuthorizationOptions.AuthorizationUri);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Add user profile's claims to the current user identity and to http headers as JWT token.
        /// </summary>
        /// <param name="userClaims">User's claims.</param>
        /// <param name="httpContext">Current Http context.</param>
        private void AddUserProfileClaimsToIdentityAndHttpHeaders(string userJwtToken)
        {
            _httpContext.Request.Headers[HeaderNames.Authorization] = $"{AuthenticationSchemes.AuthorizationHeaderBearer} {userJwtToken}";
        }
    }
}