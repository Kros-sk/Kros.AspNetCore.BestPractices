using Kros.Authorization.Api.Application.Commands.UpdatePermissions;
using Kros.Authorization.Api.Application.Model;
using Kros.Authorization.Api.Application.Queries.Permission;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Services
{
    /// <summary>
    /// Interface which describe service for working with <see cref="Permission"/>.
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Checks whether user has admin rights for organization.
        /// </summary>
        /// <param name="claims">User claims.</param>
        /// <returns>True, if user has admin rights; otherwise false.</returns>
        bool IsAdminFromClaims(ClaimsPrincipal claims);

        /// <summary>
        /// Checks whether user has writer rights for organization.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns>True, if user has writer rights; otherwise false.</returns>
        bool IsWriterFromClaims(ClaimsPrincipal claims);

        /// <summary>
        /// Checks whether user has reader rights for organization.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns>True, if user has reader rights; otherwise false.</returns>
        bool IsReaderFromClaims(ClaimsPrincipal claims);

        /// <summary>
        /// Gets user permissions related to current organization.
        /// </summary>
        /// <param name="userClaims">All user claims (contains user id and organization id).</param>
        /// <returns>User permissions.</returns>
        Task<IEnumerable<GetUserPermissionsForOrganizationQuery.Permission>> GetUserPermissionsByOrganizationAsync(
            IEnumerable<Claim> userClaims);

        /// <summary>
        /// Updates user permissions.
        /// </summary>
        /// <param name="command">Update command.</param>
        Task UpdateUserPermissionsAsync(UpdatePermissionsCommand command);
    }
}