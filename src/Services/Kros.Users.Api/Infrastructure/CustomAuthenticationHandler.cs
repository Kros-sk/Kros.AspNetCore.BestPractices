using Kros.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Kros.Users.Api.Infrastructure
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Schema name for custom authentication
        /// </summary>
        public const string CustomAuthenticationSchemaName = "CustomAuthentication";

        /// <summary>
        /// Policy name for admins.
        /// </summary>
        public const string CustomAuthorizationAdminPolicyName = "admin";

        /// <summary>
        /// Required value for admin claim.
        /// </summary>
        public const string CustomAuthorizationAdminPolicyValue = "True";

        /// <summary>
        /// Claim type for admin user role.
        /// </summary>
        public const string ClaimTypeForAdmin = "IsAdmin";


        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            KeyValuePair<string, StringValues> userIdlHttpValue = Request.Headers.FirstOrDefault(x => x.Key == "OpenIdUserId");
            if (userIdlHttpValue.Key == null)
            {
                return AuthenticateResult.Fail("Missing Authorization Header \"OpenIdUserId\"");
            }

            KeyValuePair<string, StringValues> userEmailHttpValue = Request.Headers.FirstOrDefault(x => x.Key == "OpenIdUserEmail");
            if (userEmailHttpValue.Key == null)
            {
                return AuthenticateResult.Fail("Missing Authorization Header \"OpenIdUserEmail\"");
            }

            var claims = new[] {
                new Claim(userIdlHttpValue.Key, userIdlHttpValue.Value),
                new Claim(userEmailHttpValue.Key, userEmailHttpValue.Value),
                new Claim(ClaimTypeForAdmin, CustomAuthorizationAdminPolicyValue),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}