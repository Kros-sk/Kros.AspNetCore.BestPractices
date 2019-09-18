using Kros.ToDos.Base.Infrastructure;
using System.Linq;
using System.Security.Claims;

namespace Kros.ToDos.Api.Application
{
    /// <summary>
    /// Claims principal extensions for <see cref="System.Security.Claims.ClaimsPrincipal" />.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets organization ID from user claims.
        /// </summary>
        /// <param name="claimsPrincipal">Claims principal which contains all user claims.</param>
        /// <returns>Organization ID.</returns>
        public static int GetOrganizationId(this ClaimsPrincipal claimsPrincipal)
        {
            if (int.TryParse(GetValueFromUserClaims(claimsPrincipal, PermissionsHelper.Claims.OrganizationId), out int result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Return specific value from user claims.
        /// </summary>
        /// <param name="claimsPrincipal">Claims principal which contains all user claims.</param>
        /// <param name="userClaimType">Claim type.</param>
        /// <returns>Claim value.</returns>
        private static string GetValueFromUserClaims(ClaimsPrincipal claimsPrincipal, string userClaimType)
            => claimsPrincipal.Claims.FirstOrDefault(x => x.Type == userClaimType)?.Value ?? string.Empty;
    }
}