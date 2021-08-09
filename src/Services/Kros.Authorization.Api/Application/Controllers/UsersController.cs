using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands;
using Kros.Authorization.Api.Application.Queries;
using Kros.Authorization.Api.Application.Services;
using Kros.ToDos.Base.Extensions;
using Kros.ToDos.Base.Infrastructure;
using Kros.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsersController : ApiBaseController
    {
        private readonly IUserService _userService;
        private readonly IPermissionService _permissionsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userService">User service.</param>
        /// <param name="permissionService">Permission service.</param>
        public UsersController(IUserService userService, IPermissionService permissionService)
        {
            _userService = Check.NotNull(userService, nameof(userService));
            _permissionsService = Check.NotNull(permissionService, nameof(permissionService));
        }

        /// <summary>
        /// Is user admin?
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns>Returns <see langword="true"/>, if user has admin privileges for organization.</returns>
        [HttpGet(nameof(IsAdmin))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<bool> IsAdmin()
            => await this.SendRequest(new UserAdminRightsQuery());

        /// <summary>
        /// Is user writer?
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns>Returns <see langword="true"/>, if user has writer privileges for organization.</returns>
        [HttpGet(nameof(IsWriter))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<bool> IsWriter()
            => await this.SendRequest(new UserWriterRightsQuery());

        /// <summary>
        /// Is user reader?
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns>Returns <see langword="true"/>, if user has reader privileges for organization.</returns>
        [HttpGet(nameof(IsReader))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<bool> IsReader()
            => await this.SendRequest(new UserReaderRightsQuery());

        /// <summary>
        /// Get user by id.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="403">
        /// Forbidden when user doesn't have permissions for retrieving users.
        /// </response>
        /// <returns>User.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserQuery.User))]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<GetUserQuery.User> GetUser(int userId)
            => await this.SendRequest(new GetUserQuery(userId));

        /// <summary>
        /// Get all users.
        /// Only admin can call this method.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">
        /// Forbidden when user doesn't have permissions for retrieving users.
        /// </response>
        /// <returns>All application users.</returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAllUsersQuery.User>))]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<IEnumerable<GetAllUsersQuery.User>> GetAllUsers()
            => await this.SendRequest(new GetAllUsersQuery(User.GetOrganizationId()));

        /// <summary>
        /// Update user.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="command">Data for updating user.</param>
        /// <response code="200">Updated.</response>
        /// <response code="403">Forbidden when user doesn't have permissions for editing users.</response>
        /// <returns>Return Ok, if update is successful.</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(PoliciesHelper.AdminAuthPolicyName)]
        public async Task<ActionResult> UpdateUser(int userId, UpdateUserCommand command)
        {
            command.Id = userId;
            await this.SendRequest(command);

            return Ok();
        }
    }
}
