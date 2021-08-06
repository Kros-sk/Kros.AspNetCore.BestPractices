using Kros.Authorization.Api.Application.Queries;
using Kros.Authorization.Api.Domain;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Services
{
    /// <summary>
    /// Interface which describe service for working with <see cref="User"/>.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get user by email or create it, if doesn't exist.
        /// </summary>
        /// <param name="userClaims">All user claims (contains email).</param>
        /// <returns>User.</returns>
        Task<GetUserByEmailQuery.User> TryCreateDefaultUserAsync(IEnumerable<Claim> userClaims);
    }
}
