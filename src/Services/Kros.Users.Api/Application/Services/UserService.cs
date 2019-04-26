using IdentityModel;
using Kros.Users.Api.Application.Commands;
using Kros.Users.Api.Application.Model;
using Kros.Users.Api.Application.Queries;
using Kros.Users.Api.Infrastructure;
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
        public async Task TryCreateDefaultUserAsync(ClaimsPrincipal user)
        {
            var userEmail = user.FindFirstValue(JwtClaimTypes.Email);

            if (!await UserExists(userEmail))
            {
                await CreateNewUserAsync(new CreateUserCommand()
                {
                    Email = userEmail,
                    FirstName = user.FindFirstValue(JwtClaimTypes.GivenName),
                    LastName = user.FindFirstValue(JwtClaimTypes.FamilyName),
                    IsAdmin = false
                });
            }
        }

        /// <inheritdoc />
        public bool? TryIsAdminFromClaims(ClaimsPrincipal user)
        {
            var adminClaim = user.FindFirstValue(UserProfileMiddleware.ClaimTypeForAdmin);

            if (adminClaim == null)
            {
                return null;
            }

            return bool.Parse(adminClaim);
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(UpdateUserCommand command)
        {
            await _mediator.Send(command);
            _cache.Remove(command.Email); // Removing old user values from cache
        }

        private async Task<bool> UserExists(string userEmail)
        {
            var userFromDb = await _mediator.Send(new GetUserByEmailQuery(userEmail));
            return (userFromDb != null);
        }

        private async Task CreateNewUserAsync(CreateUserCommand createUserCommand)
        {
            await _mediator.Send(createUserCommand);
        }
    }
}
