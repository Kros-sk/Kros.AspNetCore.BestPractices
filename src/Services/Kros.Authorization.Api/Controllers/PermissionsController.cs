using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands.CreatePermissions;
using Kros.Authorization.Api.Application.Commands.DeletePermissions;
using Kros.Authorization.Api.Application.Commands.UpdatePermissions;
using Kros.Authorization.Api.Application.Services;
using Kros.ToDos.Api.Application;
using Kros.ToDos.Base.Infrastructure;
using Kros.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Controllers
{
    /// <summary>
    /// Permissions controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    public class PermissionsController : ControllerBase
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
        /// <returns>Returns Ok, if update is successfull.</returns>
        [HttpPut]
        [ProducesResponseType(200)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> UpdateUserPermissions(UpdatePermissionsCommand command)
        {
            command.OrganizationId = User.GetCompanyId();

            await _permissionsService.UpdateUserPermissionsAsync(command);

            return Ok();
        }

        /// <summary>
        /// Updates user role within current organization.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="command">Data for updating user permissions.</param>
        /// <returns>Returns Ok, if update is successfull.</returns>
        [HttpPut("userRole")]
        [ProducesResponseType(200)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> UpdateUserRole(UpdatePermissionsCommand command)
        {
            command.OrganizationId = User.GetCompanyId();
            command.Key = PermissionsHelper.Claims.UserRole;

            await _permissionsService.UpdateUserPermissionsAsync(command);

            return Ok();
        }

        /// <summary>
        /// Creates or updates user role in company.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> CreateOrUpdateUserRole(CreatePermissionsCommand command)
            => await this.SendCreateCommand(command);

        /// <summary>
        /// Set user role in company to Owner.
        /// </summary>
        [HttpPost("Owner")]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateUserRoleOwner(CreatePermissionsCommand command)
        {
            command.Key = PermissionsHelper.Claims.UserRole;
            command.Value = PermissionsHelper.ClaimValues.OwnerRole;
            return await this.SendCreateCommand(command);
        }

        /// <summary>
        /// Deletes user role by company id.
        /// </summary>
        [HttpDelete("{companyId}")]
        [ProducesResponseType(204)]
        [Authorize(PoliciesHelper.OwnerAuthPolicyName)]
        public async Task<ActionResult> DeleteAllUsersRoleByCompany(long companyId)
        {
            await this.SendRequest(new DeleteAllPermissionsByCompanyCommand(companyId));

            return NoContent();
        }

        /// <summary>
        /// Deletes user role by company id and user id.
        /// </summary>
        [HttpDelete("{companyId}/user/{userId}")]
        [ProducesResponseType(204)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> DeleteUserRoleByCompany(long companyId, long userID)
        {
            await this.SendRequest(new DeleteUserPermissionsByCompanyCommand(companyId, userID));

            return NoContent();
        }

    }
}