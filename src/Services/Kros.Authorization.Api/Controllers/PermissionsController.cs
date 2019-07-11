using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands.UpdatePermissions;
using Kros.Authorization.Api.Application.Services;
using Kros.Authorization.Api.Infrastructure;
using Kros.ToDos.Api.Application;
using Kros.ToDos.Api.Infrastructure;
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
    }
}