using Kros.Users.Api.Application.Commands;
using Kros.Users.Api.Application.Model;
using Kros.Users.Api.Application.Queries;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Application.Services
{
    /// <summary>
    /// Interface which describe service for working with <see cref="User"/>.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Is user admin?
        /// </summary>
        /// <param name="user">User for detect.</param>
        /// <returns>true, if it's admin, false otherwise.</returns>
        bool IsAdminFromClaims(ClaimsPrincipal user);

        /// <summary>
        /// Try create new user.
        /// </summary>
        /// <param name="user"></param>
        Task<GetUserByEmailQuery.User> TryCreateDefaultUserAsync(ClaimsPrincipal user);

        /// <summary>
        /// Update user values;
        /// </summary>
        /// <param name="command">Update command.</param>
        Task UpdateUserAsync(UpdateUserCommand command);
    }
}
