using IdentityModel;
using Kros.Users.Api.Application.Commands;
using Kros.Users.Api.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class UsersController : ControllerBase
    {
        [HttpGet("IsAdmin")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> IsAdmin()
        {
            var isUserAdmin = this.User.FindFirstValue("IsAdmin");

            if (isUserAdmin == null)
            {
                var userEmail = this.User.FindFirstValue(JwtClaimTypes.Email);

                // User exists?
                var user = await this.SendRequest(new GetUserByEmailQuery(userEmail));

                if (user == null)
                {
                    // Create new user
                    var newUserCommand = new CreateUserCommand();
                    newUserCommand.Email = userEmail;
                    newUserCommand.IsAdmin = false;
                    await this.SendCreateCommand(newUserCommand, nameof(GetUser));
                }

                return false;
            }

            return bool.Parse(isUserAdmin);
        }

        [HttpGet("GetUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(GetUserQuery.User))]
        [Authorize("admin")]
        public async Task<GetUserQuery.User> GetUser(int userId)
        {
            return await this.SendRequest(new GetUserQuery(userId));
        }

        [HttpGet("GetAllUsers")]
        [Authorize("admin")]
        public async Task<IEnumerable<GetAllUsersQuery.User>> GetAllUsers()
        {
            return await this.SendRequest(new GetAllUsersQuery());
        }
    }
}
