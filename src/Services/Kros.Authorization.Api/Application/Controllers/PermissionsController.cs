using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands;
using Kros.Authorization.Api.Application.Services;
using Kros.ToDos.Base.Extensions;
using Kros.ToDos.Base.Infrastructure;
using Kros.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Controllers
{
    /// <summary>
    /// Permissions controller.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PermissionsController : ApiBaseController
    {
        private readonly IPermissionService _permissionsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="permissionService">Permission service.</param>
        public PermissionsController(IPermissionService permissionService)
        {
            _permissionsService = Check.NotNull(permissionService, nameof(permissionService));
        }

        /// <summary>
        /// Updates user permissions within current organization.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="command">Data for updating user permissions.</param>
        /// <response code="200">Ok.</response>
        /// <response code="403">User is not authorized to update user permissions.</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> UpdateUserPermissions(UpdatePermissionsCommand command)
        {
            command.OrganizationId = User.GetOrganizationId();
            await this.SendRequest(command);

            return Ok();
        }

        /// <summary>
        /// Updates user role within current organization.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="command">Data for updating user role.</param>
        /// <response code="200">Ok.</response>
        /// <response code="403">User is not authorized to update user roles.</response>
        [HttpPut("userRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> UpdateUserRole(UpdatePermissionsCommand command)
        {
            command.OrganizationId = User.GetOrganizationId();
            command.Key = PermissionsHelper.Claims.UserRole;

            await this.SendRequest(command);

            return Ok();
        }

        /// <summary>
        /// Creates or updates user permissions in organization.
        /// </summary>
        /// <param name="command">Data for creating/updating user permissions.</param>
        /// <response code="201">Created.</response>
        /// <response code="403">
        /// Forbidden when user doesn't have permissions for creating/updating user permissions.
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> CreateOrUpdatePermissions(CreatePermissionsCommand command)
            => await this.SendCreateCommand(command);

        /// <summary>
        /// Set user's role in organization to Owner.
        /// </summary>
        /// <param name="command">Data for setting user's role.</param>
        /// <response code="201">Created.</response>
        [HttpPost("Owner")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateOwner(CreatePermissionsCommand command)
        {
            command.Key = PermissionsHelper.Claims.UserRole;
            command.Value = PermissionsHelper.ClaimValues.OwnerRole;
            return await this.SendCreateCommand(command);
        }

        /// <summary>
        /// Deletes all user permissions in organization.
        /// </summary>
        /// <param name="organizationId">Organization ID.</param>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permissions for deleting user permissions.</response>
        [HttpDelete("{organizationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(PoliciesHelper.OwnerAuthPolicyName)]
        public async Task<ActionResult> DeleteAllPermissions(long organizationId)
        {
            await this.SendRequest(new DeleteAllPermissionsByOrganizationCommand(organizationId));

            return NoContent();
        }

        /// <summary>
        /// Deletes user permissions in organization.
        /// </summary>
        /// <param name="organizationId">Organization ID.</param>
        /// <param name="userID">User ID.</param>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permissions for deleting user permissions.</response>
        [HttpDelete("{organizationId}/user/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> DeleteUserPermissions(long organizationId, long userID)
        {
            await this.SendRequest(new DeleteUserPermissionsByOrganizationCommand(organizationId, userID));

            return NoContent();
        }
    }
}
