﻿using Flurl;
using IdentityModel;
using IdentityModel.Client;
using Kros.Identity.Extensions;
using Kros.Users.Api.Application.Queries;
using Kros.Utils;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Infrastructure
{
    /// <summary>
    /// Middleware for user profile.
    /// </summary>
    public class UserProfileMiddleware
    {
        /// <summary>
        /// Claim type for admin user role.
        /// </summary>
        public const string ClaimTypeForAdmin = "IsAdmin";

        /// <summary>
        /// Http client name for communication with Identity Server.
        /// </summary>
        public const string IdentityServerHttpClientName = "IdentityServerClient";
        private const string IdentityServerUserInfoEndpoint = "connect/userinfo";

        private readonly RequestDelegate _next;
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly IMemoryCache _cache;
        private IHttpClientFactory _httpClientFactory;
        private IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        /// <param name="cache">Cache service.</param>
        /// <param name="identityServerOptions">Identity Server options.</param>
        public UserProfileMiddleware(
            RequestDelegate next,
            IMemoryCache cache,
            IdentityServerOptions identityServerOptions)
        {
            _next = Check.NotNull(next, nameof(next));
            _cache = Check.NotNull(cache, nameof(cache));
            _identityServerOptions = identityServerOptions;
        }

        /// <summary>
        /// HttpContext pipeline processing.
        /// </summary>
        /// <param name="httpContext">Http context.</param>
        /// <param name="mediator">Mediator service.</param>
        /// <param name="httpClientFactory">Http client factory.</param>
        public async Task Invoke(
            HttpContext httpContext,
            IMediator mediator,
            IHttpClientFactory httpClientFactory)
        {
            _mediator = mediator;
            _httpClientFactory = httpClientFactory;

            var userClaims = await GetUserProfileClaimsAsync(httpContext);
            await AddUserProfileClaimsToCurrentIdentityAsync(userClaims, httpContext);

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
                    OidcConstants.AuthenticationSchemes.FormPostBearer);

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
        private async Task AddUserProfileClaimsToCurrentIdentityAsync(
            IEnumerable<Claim> userClaims,
            HttpContext httpContext)
        {
            Claim emailClaim = userClaims?.FirstOrDefault(x => x.Type == JwtClaimTypes.Email);

            if (emailClaim != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddClaim(emailClaim);
                claimsIdentity.AddClaim(userClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName));
                claimsIdentity.AddClaim(userClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName));

                var isAdmin = await IsUserAdminAsync(emailClaim.Value);

                if (isAdmin != null)
                {
                    Claim adminClaim = new Claim(ClaimTypeForAdmin, isAdmin.Value.ToString());
                    claimsIdentity.AddClaim(adminClaim);
                }

                httpContext.User?.AddIdentity(claimsIdentity);
            }
        }

        private async Task<bool?> IsUserAdminAsync(string userEmail)
        {
            if (!_cache.TryGetValue(userEmail, out bool? isAdmin))
            {
                var user = await _mediator.Send(new GetUserByEmailQuery(userEmail));

                if (user != null)
                {
                    isAdmin = user.IsAdmin;
                    _cache.Set(userEmail, isAdmin);
                }
            }

            return isAdmin;
        }
    }
}
