using IdentityModel.Client;
using IdentityServer4.Stores.Serialization;
using Kros.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
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
        private readonly JwtAuthorizationSecurityOptions _jwtAuthorizationSecurityOptions;
        private IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        /// <param name="cache">Cache service.</param>
        /// <param name="identityServerOptions">Identity Server options.</param>
        public AuthorizationMiddleware(
            RequestDelegate next,
            JwtAuthorizationSecurityOptions jwtAuthorizationSecurityOptions)
        {
            _next = Check.NotNull(next, nameof(next));
            _jwtAuthorizationSecurityOptions = Check.NotNull(jwtAuthorizationSecurityOptions, nameof(jwtAuthorizationSecurityOptions));
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

            var oidcUserClaims = await GetOidcUserClaimsAsync(httpContext);
            var oicdUserJwtToken = CreateJwtTokenFromClaims(oidcUserClaims);
            var userAuthorizationClaims = await GetUserAuthorizationClaimsAsync(httpContext, oicdUserJwtToken);

            var allUserClaims = new List<Claim>();
            allUserClaims.AddRange(oidcUserClaims);
            allUserClaims.AddRange(userAuthorizationClaims);

            var userJwtToken = CreateJwtTokenFromClaims(allUserClaims);
            AddUserProfileClaimsToIdentityAndHttpHeaders(userJwtToken, httpContext);

            await _next(httpContext);
        }

        private string CreateJwtTokenFromClaims(IEnumerable<Claim> userClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtAuthorizationSecurityOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            try
            {
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(securityToken);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task<IEnumerable<Claim>> GetOidcUserClaimsAsync(HttpContext httpContext)
        {
            string token = await httpContext.GetTokenAsync(AuthenticationSchemes.FormPostBearer); // FormPostBearer == "access_token"

            if (token != null)
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var response = await client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = _jwtAuthorizationSecurityOptions.IdentityServerUserInfoEndpoint,
                        Token = token
                    });

                    if (!response.IsError)
                    {
                        return response.Claims;
                    }
                }
            }

            return new List<Claim>();
        }

        /// <summary>
        /// Get user profile's claims from IdentityServer user profile endpoint.
        /// </summary>
        /// <param name="httpContext">Current Http context.</param>
        /// <returns>User profile's claims.</returns>
        private async Task<IEnumerable<Claim>> GetUserAuthorizationClaimsAsync(HttpContext httpContext, string jwtToken)
        {
            string token = await httpContext.GetTokenAsync(AuthenticationSchemes.FormPostBearer); // FormPostBearer == "access_token"

            if (token != null)
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue(AuthenticationSchemes.AuthorizationHeaderBearer, jwtToken);
                    var response = await client.GetAsync(_jwtAuthorizationSecurityOptions.UserClaimsEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<IEnumerable<Claim>>(result, new ClaimConverter());
                    }
                }
            }

            return new List<Claim>();
        }

        /// <summary>
        /// Add user profile's claims to the current user identity and to http headers as JWT token.
        /// </summary>
        /// <param name="userClaims">User's claims.</param>
        /// <param name="httpContext">Current Http context.</param>
        private void AddUserProfileClaimsToIdentityAndHttpHeaders(
            string userJwtToken,
            HttpContext httpContext)
        {
            httpContext.Request.Headers[HeaderNames.Authorization] = $"{AuthenticationSchemes.AuthorizationHeaderBearer} {userJwtToken}";
        }
    }
}