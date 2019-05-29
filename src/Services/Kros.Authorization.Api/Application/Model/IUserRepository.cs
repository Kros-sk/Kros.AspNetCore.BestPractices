using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Model
{
    /// <summary>
    /// Interface which describe repository for persistating <see cref="User"/>.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Create new user in repository.
        /// </summary>
        /// <param name="user">Creating user.</param>
        Task CreateUserAsync(User user);

        /// <summary>
        /// Update user in repository.
        /// </summary>
        /// <param name="user">Updating user.</param>
        Task UpdateUserAsync(User user);
    }
}
