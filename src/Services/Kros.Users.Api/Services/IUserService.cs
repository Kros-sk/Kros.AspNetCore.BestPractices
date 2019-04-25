using Kros.Users.Api.Application.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kros.Users.Api.Services
{
    /// <summary>
    /// Interface which describe service for working with <see cref="User"/>.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Logged user with claims.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> IsCurrentUserAdminAsync(ClaimsPrincipal user);
    }
}
