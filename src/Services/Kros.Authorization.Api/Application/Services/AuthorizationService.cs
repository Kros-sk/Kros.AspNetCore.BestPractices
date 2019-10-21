using IdentityModel.Client;
using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Queries;
using Kros.ToDos.Base.Infrastructure;
using Kros.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace Kros.Authorization.Api.Application.Services
{
    /// <summary>
    /// Service for user authorization.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiJwtAuthorizationOptions _apiJwtAuthorizationOptions;
        private readonly JwtAuthorizationOptions _jwtAuthorizationOptions;
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory (without name).</param>
        /// <param name="jwtAuthorizationOptions">Authorization options for authorization service.</param>
        /// <param name="apiJwtAuthorizationOptions">Authorization options for api service.</param>
        /// <param name="userService">User service.</param>
        /// <param name="permissionService">Permission service.</param>
        /// <param name="httpContextAccessor">Httpc context accessor</param>
        public AuthorizationService(
            IHttpClientFactory httpClientFactory,
            IOptions<JwtAuthorizationOptions> jwtAuthorizationOptions,
            IOptions<ApiJwtAuthorizationOptions> apiJwtAuthorizationOptions,
            IUserService userService,
            IPermissionService permissionService,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = Check.NotNull(httpClientFactory, nameof(httpClientFactory));
            _apiJwtAuthorizationOptions = Check.NotNull(apiJwtAuthorizationOptions, nameof(apiJwtAuthorizationOptions)).Value;
            _jwtAuthorizationOptions = Check.NotNull(jwtAuthorizationOptions, nameof(jwtAuthorizationOptions)).Value;
            _userService = Check.NotNull(userService, nameof(userService));
            _permissionService = Check.NotNull(permissionService, nameof(permissionService));
            _httpContextAccessor = Check.NotNull(httpContextAccessor, nameof(httpContextAccessor));
        }

        /// <inheritdoc />
        public async Task<string> CreateJwtTokenAsync()
        {
            var oidcClaims = await GetOidcUserClaimsAsync();
            var user = await _userService.TryCreateDefaultUserAsync(oidcClaims);

            if (user != null)
            {
                var allUserClaims = new List<Claim>();
                allUserClaims.AddRange(oidcClaims);
                allUserClaims.AddRange(GetUserAuthorizationClaims(user));
                allUserClaims.AddRange(GetOrganizationClaims(_httpContextAccessor.HttpContext.Request.Headers));
                allUserClaims.AddRange(await GetPermissionsClaimsAsync(allUserClaims));

                return JwtAuthorizationHelper.CreateJwtTokenFromClaims(allUserClaims, _apiJwtAuthorizationOptions.JwtSecret);
            }

            throw new UnauthorizedAccessException(Properties.Resources.AuthorizationServiceForbiddenRequest);
        }

        private IEnumerable<Claim> GetUserAuthorizationClaims(GetUserByEmailQuery.User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(UserClaimTypes.UserId, user.Id.ToString()));
            //claims.Add(new Claim(UserClaimTypes.IsAdmin, user.IsAdmin.ToString()));

            return claims;
        }

        private async Task<IEnumerable<Claim>> GetOidcUserClaimsAsync()
        {
            string accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(
                JwtAuthorizationHelper.OAuthSchemeName, AuthenticationSchemes.FormPostBearer);

            if (accessToken != null)
            {
                using (var client = _httpClientFactory.CreateClient())
                {
                    var response = await client.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = _jwtAuthorizationOptions.IdentityServerUserInfoEndpoint,
                        Token = accessToken
                    });

                    if (!response.IsError)
                    {
                        return response.Claims;
                    }
                }
            }

            return Enumerable.Empty<Claim>();
        }

        /// <summary>
        /// Gets organization-related claims.
        /// </summary>
        /// <param name="headers">HTTP request headers.</param>
        /// <returns>Organization-related claims.</returns>
        private IEnumerable<Claim> GetOrganizationClaims(IHeaderDictionary headers)
        {
            if (headers != null && headers.ContainsKey(PermissionsHelper.Headers.OrganizationId))
            {
                return new Claim[] { new Claim(PermissionsHelper.Claims.OrganizationId, headers[PermissionsHelper.Headers.OrganizationId]) };
            }
            else
            {
                return new Claim[] { };
            }
        }

        /// <summary>
        /// Gets user permissions and returns them as claims.
        /// </summary>
        /// <param name="userClaims">Current user claims.</param>
        /// <returns>Claims based on user permissions.</returns>
        private async Task<IEnumerable<Claim>> GetPermissionsClaimsAsync(IEnumerable<Claim> userClaims)
        {
            var userPermissions = await _permissionService.GetUserPermissionsByOrganizationAsync(userClaims);
            return userPermissions?.Select(p => new Claim(p.Key, p.Value)).AsEnumerable() ?? new Claim[] { };
        }
    }
}
