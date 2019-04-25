using IdentityModel;
using Kros.Users.Api.Application.Commands.CreateUser;
using Kros.Users.Api.Application.Commands.UpdateUser;
using Kros.Users.Api.Application.Queries;
using Kros.Users.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="userService">User service.</param>
        /// <param name="cache">Cache.</param>
        public UsersController(
            IUserService userService,
            IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        /// <summary>
        /// Is user admin?
        /// </summary>
        /// <returns>Return true, if user is admin.</returns>
        [HttpGet(nameof(IsAdmin))]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> IsAdmin()
        {
            return await _userService.IsCurrentUserAdminAsync(this.User);
        }

        /// <summary>
        /// Get user by id.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>User.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(GetUserQuery.User))]
        [Authorize("admin")]
        public async Task<GetUserQuery.User> GetUser(int userId)
        {
            return await this.SendRequest(new GetUserQuery(userId));
        }

        /// <summary>
        /// Get all users.
        /// Only admin can call this method.
        /// </summary>
        /// <returns>All application users.</returns>
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAllUsersQuery.User>))]
        [Authorize("admin")]
        public async Task<IEnumerable<GetAllUsersQuery.User>> GetAllUsers()
        {
            return await this.SendRequest(new GetAllUsersQuery());
        }

        /// <summary>
        /// Update user.
        /// Only admin can call this method.
        /// </summary>
        /// <param name="command">Data for updating user.</param>
        /// <param name="userId">User id.</param>
        /// <returns>Return true, if update is success.</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(200)]
        [Authorize("admin")]
        public async Task<ActionResult> UpdateUser(int userId, UpdateUserCommand command)
        {
            command.Id = userId;

            await this.SendRequest(command);

            _cache.Remove(command.Email); // Removing old user values from cache

            return Ok();
        }
    }
}
