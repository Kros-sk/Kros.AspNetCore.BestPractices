using IdentityModel;
using IdentityModel.Client;
using Kros.Identity.Extensions;
using Kros.Users.Api.Application.Queries;
using Kros.Utils;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Middlewares
{
    /// <summary>
    /// Middleware for user profile.
    /// </summary>
    public class UserProfileMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="next">The next.</param>
        public UserProfileMiddleware(
            RequestDelegate next, 
            IOptions<IdentityServerOptions> identityServerOptions,
            IMemoryCache cache)
        {
            _next = Check.NotNull(next, nameof(next));
            _identityServerOptions = identityServerOptions.Value;
            _cache = cache;
        }

        /// <summary>
        /// HttpContext pipeline processing.
        /// </summary>
        /// <param name="httpContext">Http context.</param>
        public async Task Invoke(HttpContext httpContext)
        {
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
            if (httpContext.User != null)
            {
                string token = await httpContext.GetTokenAsync("access_token");

                if (token != null)
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetUserInfoAsync(new UserInfoRequest
                        {
                            Address = $"{_identityServerOptions.AuthorityUrl}/connect/userinfo",
                            Token = token
                        });

                        if (!response.IsError)
                        {
                            return response.Claims;
                        }
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
        private async Task AddUserProfileClaimsToCurrentIdentityAsync(IEnumerable<Claim> userClaims, HttpContext httpContext)
        {
            Claim claim = userClaims?.FirstOrDefault(x => x.Type == JwtClaimTypes.Email);

            if (claim != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddClaim(claim);

                var isAdmin = await IsUserAdminAsync(claim.Value, httpContext);

                if (isAdmin != null)
                {
                    Claim adminClaim = new Claim("isAdmin", isAdmin.Value.ToString());
                    claimsIdentity.AddClaim(adminClaim);
                }
                
                httpContext.User?.AddIdentity(claimsIdentity);
            }
        }

        private async Task<bool?> IsUserAdminAsync(string userEmail, HttpContext httpContext)
        {
            if (!_cache.TryGetValue(userEmail, out bool? isAdmin))
            {
                var mediator = GetMediatorService(httpContext);
                var user = await mediator.Send(new GetUserByEmailQuery(userEmail));

                if (user != null)
                {
                    isAdmin = user.IsAdmin;
                    _cache.Set(userEmail, isAdmin);
                }
            }

            return isAdmin;
        }

        private IMediator GetMediatorService(HttpContext httpContext)
        {
            return (IMediator)httpContext.RequestServices.GetService(typeof(IMediator));
        }
    }
}
