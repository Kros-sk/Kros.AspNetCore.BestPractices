using Kros.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiGateway.Infrastructure
{
    /// <summary>
    /// Middleware for transformation from http headers to user claims.
    /// </summary>
    public class TransformHeadersToClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="next">Next middleware.</param>
        public TransformHeadersToClaimsMiddleware(RequestDelegate next)
        {
            _next = Check.NotNull(next, nameof(next));
        }

        /// <summary>
        /// HttpContext pipeline processing.
        /// </summary>
        /// <param name="httpContext">Http context.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            TransformHttpHeadersToUserClaims(httpContext);

            await _next(httpContext);
        }

        /// <summary>
        /// Transformation from http headers to user claims.
        /// </summary>
        /// <param name="httpContext">Current http context.</param>
        private void TransformHttpHeadersToUserClaims(HttpContext httpContext)
        {
            KeyValuePair<string, StringValues> userIdlHttpValue = httpContext.Request.Headers.FirstOrDefault(x => x.Key == "OpenIdUserId");
            KeyValuePair<string, StringValues> userEmailHttpValue = httpContext.Request.Headers.FirstOrDefault(x => x.Key == "OpenIdUserEmail");

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            if (userIdlHttpValue.Key != null)
            {
                claimsIdentity.AddClaim(new Claim(userIdlHttpValue.Key, userIdlHttpValue.Value));
            }

            if (userEmailHttpValue.Key != null)
            {
                claimsIdentity.AddClaim(new Claim(userEmailHttpValue.Key, userEmailHttpValue.Value));
            }

            httpContext.User?.AddIdentity(claimsIdentity);
        }
    }
}