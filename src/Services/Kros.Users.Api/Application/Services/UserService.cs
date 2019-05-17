using Kros.Users.Api.Application.Commands;
using Kros.Users.Api.Application.Model;
using Kros.Users.Api.Application.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Application.Services
{
    /// <summary>
    /// Service for working with <see cref="User"/>.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="mediator">Mediator service.</param>
        /// <param name="cache">Cache service.</param>
        public UserService(
            IMediator mediator,
            IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<GetUserByEmailQuery.User> TryCreateDefaultUserAsync(ClaimsPrincipal userClaims)
        {
            var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);
            var user = await GetUserAsync(userEmail);

            if (user == null)
            {
                user = new GetUserByEmailQuery.User()
                {
                    Email = userEmail,
                    IsAdmin = false
                };

                user.Id = await CreateNewUserAsync(new CreateUserCommand()
                {
                    Email = userEmail,
                    FirstName = userClaims.FindFirstValue(ClaimTypes.GivenName),
                    LastName = userClaims.FindFirstValue(ClaimTypes.Surname),
                    IsAdmin = false
                });
            }

            return user;
        }

        /// <inheritdoc />
        public bool IsAdminFromClaims(ClaimsPrincipal user)
        {
            string adminClaim = user.FindFirstValue("IsAdmin");

            if (adminClaim == null)
            {
                return false;
            }

            return bool.Parse(adminClaim);
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(UpdateUserCommand command)
        {
            await _mediator.Send(command);
            _cache.Remove(command.Email); // Removing old user values from cache
        }

        private async Task<GetUserByEmailQuery.User> GetUserAsync(string userEmail)
        {
            return await _mediator.Send(new GetUserByEmailQuery(userEmail));
        }

        private async Task<int> CreateNewUserAsync(CreateUserCommand createUserCommand)
        {
            return await _mediator.Send(createUserCommand);
        }
    }
}
