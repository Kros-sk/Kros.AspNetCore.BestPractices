using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands.UpdatePermissions;
using Kros.Authorization.Api.Application.Model;
using Kros.Authorization.Api.Application.Queries.Permission;
using Kros.Authorization.Api.Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Services
{
    /// <summary>
    /// Service for working with <see cref="Permission"/>.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="mediator">Mediator service.</param>
        public PermissionService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public bool IsOwnerFromClaims(ClaimsPrincipal claims)
            => CheckUserRoles(claims, PermissionsHelper.ClaimValues.OwnerRole);

        /// <inheritdoc />
        public bool IsAdminFromClaims(ClaimsPrincipal claims)
            => CheckUserRoles(claims, PermissionsHelper.ClaimValues.OwnerRole,
                                      PermissionsHelper.ClaimValues.AdminRole);

        /// <inheritdoc />
        public bool IsWriterFromClaims(ClaimsPrincipal claims)
            => CheckUserRoles(claims, PermissionsHelper.ClaimValues.OwnerRole,
                                      PermissionsHelper.ClaimValues.AdminRole,
                                      PermissionsHelper.ClaimValues.WriterRole);

        /// <inheritdoc />
        public bool IsReaderFromClaims(ClaimsPrincipal claims)
            => CheckUserRoles(claims, PermissionsHelper.ClaimValues.OwnerRole,
                                      PermissionsHelper.ClaimValues.AdminRole,
                                      PermissionsHelper.ClaimValues.WriterRole,
                                      PermissionsHelper.ClaimValues.ReaderRole);

        private bool CheckUserRoles(ClaimsPrincipal claims, params string[] userRoles)
        {
            string userRoleClaim = claims.FindFirstValue(PermissionsHelper.Claims.UserRole);

            if (userRoleClaim == null)
            {
                return false;
            }

            return userRoles.Contains(userRoleClaim);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetUserPermissionsForOrganizationQuery.Permission>> GetUserPermissionsByOrganizationAsync(
            IEnumerable<Claim> userClaims)
        {
            var userIdClaim = userClaims.FirstOrDefault(x => x.Type == UserClaimTypes.UserId);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var organizationIdClaim = userClaims.FirstOrDefault(x => x.Type == PermissionsHelper.Claims.OrganizationId);

                if (organizationIdClaim != null && int.TryParse(organizationIdClaim.Value, out int organizationId))
                {
                    return await _mediator.Send(new GetUserPermissionsForOrganizationQuery(userId, organizationId));
                }
            }

            return null;
        }

        /// <inheritdoc />
        public async Task UpdateUserPermissionsAsync(UpdatePermissionsCommand command)
        {
            await _mediator.Send(command);
        }
    }
}