using IdentityModel;
using Kros.Users.Api.Application.Commands.CreateUser;
using Kros.Users.Api.Application.Model;
using Kros.Users.Api.Application.Queries;
using MediatR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Services
{
    /// <summary>
    /// Service for working with <see cref="User"/>.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="mediator">Mediator service</param>
        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<bool> IsCurrentUserAdminAsync(ClaimsPrincipal user)
        {
            var isUserAdminFromClaims = IsUserAdminFromClaims(user);

            if (isUserAdminFromClaims == null)
            {
                if (!await UserExists(user))
                {
                    await CreateNewUserAsync(new CreateUserCommand()
                    {
                        Email = user.FindFirstValue(JwtClaimTypes.Email),
                        FirstName = user.FindFirstValue(JwtClaimTypes.GivenName),
                        LastName = user.FindFirstValue(JwtClaimTypes.FamilyName),
                        IsAdmin = false
                    });
                }

                return false;
            }

            return (isUserAdminFromClaims ?? false);
        }

        private bool? IsUserAdminFromClaims(ClaimsPrincipal user)
        {
            var adminClaim = user.FindFirstValue(Extensions.ServiceCollectionExtensions.ClaimTypeForAdmin);

            if (adminClaim == null)
            {
                return null;
            }

            return bool.Parse(adminClaim);
        }

        private async Task<bool> UserExists(ClaimsPrincipal user)
        {
            var userEmail = user.FindFirstValue(JwtClaimTypes.Email);
            var userFromDb = await _mediator.Send(new GetUserByEmailQuery(userEmail));

            return (userFromDb != null);
        }

        private async Task CreateNewUserAsync(CreateUserCommand createUserCommand)
        {
            await _mediator.Send(createUserCommand);
        }
    }
}
