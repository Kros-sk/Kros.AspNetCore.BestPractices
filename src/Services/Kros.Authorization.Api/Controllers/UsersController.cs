using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands;
using Kros.Authorization.Api.Application.Queries;
using Kros.Authorization.Api.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName) ]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userService">User service.</param>
        public UsersController(IUserService userService)
            => _userService = userService;

        /// <summary>
        /// Is user admin?
        /// </summary>
        /// <returns>Return true, if user is admin.</returns>
        [HttpGet(nameof(IsAdmin))]
        [ProducesResponseType(200, Type = typeof(bool))]
        public bool IsAdmin()
        {
            return _userService.IsAdminFromClaims(User);
        }

        /// <summary>
        /// Get user by id.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>User.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(GetUserQuery.User))]
        [Authorize(JwtAuthorizationHelper.AuthPolicyName)]
        public async Task<GetUserQuery.User> GetUser(int userId)
            => await this.SendRequest(new GetUserQuery(userId));

        /// <summary>
        /// Get all users.
        /// Only admin can call this method.
        /// </summary>
        /// <returns>All application users.</returns>
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAllUsersQuery.User>))]
        [Authorize(JwtAuthorizationHelper.AuthPolicyName)]
        public async Task<IEnumerable<GetAllUsersQuery.User>> GetAllUsers()
            => await this.SendRequest(new GetAllUsersQuery());

        /// <summary>
        /// Update user.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="command">Data for updating user.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Return Ok, if update is success.</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [Authorize(JwtAuthorizationHelper.AuthPolicyName)]
        public async Task<ActionResult> UpdateUser(
            int userId,
            UpdateUserCommand command)
        {
            command.Id = userId;

            await _userService.UpdateUserAsync(command);

            return Ok();
        }
    }
}
