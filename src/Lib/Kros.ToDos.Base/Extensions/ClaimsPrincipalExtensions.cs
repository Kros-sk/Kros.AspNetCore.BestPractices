using System.Linq;
using System.Security.Claims;
using static Kros.ToDos.Base.Infrastructure.PermissionsHelper;

namespace Kros.ToDos.Base.Extensions
{
    /// <summary>
    /// Claims principal extensions for <see cref="System.Security.Claims.ClaimsPrincipal" />.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets Organization id from user claims.
        /// </summary>
        /// <param name="claimsPrincipal">Claims principal which contains all user claims.</param>
        /// <returns>Organization id.</returns>
        public static int GetOrganizationId(this ClaimsPrincipal claimsPrincipal)
        {
            if (int.TryParse(GetValueFromUserClaims(claimsPrincipal, Claims.OrganizationId), out int result))
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
