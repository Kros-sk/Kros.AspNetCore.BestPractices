using Kros.AspNetCore.Authorization;
using Kros.Authorization.Api.Application.Commands;
using Kros.Authorization.Api.Application.Queries;
using Kros.Authorization.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Services
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
        /// <param name="mediator">Mediator service.</param>
        public UserService(IMediator mediator)
        {
            _mediator = Check.NotNull(mediator, nameof(mediator));
        }

        /// <inheritdoc />
        public async Task<GetUserByEmailQuery.User> TryCreateDefaultUserAsync(IEnumerable<Claim> userClaims)
        {
            var userEmailClaim = userClaims.FirstOrDefault(x => x.Type == UserClaimTypes.Email);

            if (userEmailClaim != null)
            {
                var userEmail = userEmailClaim.Value;
                var user = await GetUserAsync(userEmail);

                if (user == null)
                {
                    user = new GetUserByEmailQuery.User()
                    {
                        Email = userEmail,
                    };

                    user.Id = await CreateNewUserAsync(new CreateUserCommand()
                    {
                        Email = user.Email,
                        FirstName = userClaims.FirstOrDefault(x => x.Type == UserClaimTypes.GivenName)?.Value,
                        LastName = userClaims.FirstOrDefault(x => x.Type == UserClaimTypes.FamilyName)?.Value
                    });
                }

                return user;
            }

            return null;
        }

        private Task<GetUserByEmailQuery.User> GetUserAsync(string userEmail)
            => _mediator.Send(new GetUserByEmailQuery(userEmail));

        private Task<int> CreateNewUserAsync(CreateUserCommand createUserCommand)
            => _mediator.Send(createUserCommand);
    }
}
