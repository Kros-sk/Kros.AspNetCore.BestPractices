using Flurl;
using IdentityModel;
using IdentityModel.Client;
using Kros.Identity.Extensions;
using Kros.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

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
        private readonly AppSettingsOptions _appSettingsOptions;
        private IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        /// <param name="cache">Cache service.</param>
        /// <param name="identityServerOptions">Identity Server options.</param>
        public UserProfileMiddleware(
            RequestDelegate next,
            IdentityServerOptions identityServerOptions,
            AppSettingsOptions appSettingsOptions)
        {
            _next = Check.NotNull(next, nameof(next));
            _identityServerOptions = identityServerOptions;
            _appSettingsOptions = appSettingsOptions;
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

            var userProfileClaims = await GetUserProfileClaimsAsync(httpContext);
            if (userProfileClaims != null)
            {
                AddUserProfileClaimsToIdentityAndHttpHeaders(userProfileClaims, httpContext);
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
        /// Add user profile's claims to the current user identity and to http headers as JWT token.
        /// </summary>
        /// <param name="userClaims">User's claims.</param>
        /// <param name="httpContext">Current Http context.</param>
        private void AddUserProfileClaimsToIdentityAndHttpHeaders(
            IEnumerable<Claim> userProfileClaims,
            HttpContext httpContext)
        {
            ClaimsIdentity claimsIdentity = CreateUserClaimsIdentity(userProfileClaims);
            AddClaimsToUserIdentity(httpContext, claimsIdentity);
            AddJwtTokenToHttpHeaders(httpContext, CreateUserJwtToken(claimsIdentity));
        }

        private void AddClaimsToUserIdentity(HttpContext httpContext, ClaimsIdentity claimsIdentity)
        {
            httpContext.User?.AddIdentity(claimsIdentity);
        }

        private string CreateUserJwtToken(ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettingsOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        private void AddJwtTokenToHttpHeaders(HttpContext httpContext, string token)
        {
            httpContext.Request.Headers.Add(HeaderNames.Authorization, $"{AuthenticationSchemes.AuthorizationHeaderBearer} {token}");
        }

        private ClaimsIdentity CreateUserClaimsIdentity(IEnumerable<Claim> userClaims)
        {
            ClaimsIdentity claims = new ClaimsIdentity();

            Claim subClaim = userClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Subject);
            if (subClaim != null)
            {
                claims.AddClaim(subClaim);
            }

            Claim emailClaim = userClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email);
            if (emailClaim != null)
            {
                claims.AddClaim(emailClaim);
            }

            return claims;
        }
    }
}