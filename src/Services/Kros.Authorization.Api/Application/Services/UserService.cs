using Kros.Authorization.Api.Application.Commands;
using Kros.Authorization.Api.Application.Model;
using Kros.Authorization.Api.Application.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Kros.AspNetCore.Authorization;

namespace Kros.Authorization.Api.Application.Services
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
                        IsAdmin = false
                    };

                    user.Id = await CreateNewUserAsync(new CreateUserCommand()
                    {
                        Email = user.Email,
                        FirstName = userClaims.FirstOrDefault(x => x.Type == UserClaimTypes.GivenName)?.Value,
                        LastName = userClaims.FirstOrDefault(x => x.Type == UserClaimTypes.FamilyName)?.Value,
                        IsAdmin = false
                    });
                }
            }

            return null;
        }

        /// <inheritdoc />
        public bool IsAdminFromClaims(ClaimsPrincipal user)
        {
            string adminClaim = user.FindFirstValue(UserClaimTypes.IsAdmin);

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
