using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Services
{
    /// <summary>
    /// Interface for user role service.
    /// </summary>
    public interface IUserRoleService
    {
        /// <summary>
        /// Create owner role for user in organization.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        Task CreateOwnerRoleAsync(long userId, long organizationId);

        /// <summary>
        /// Delete user role in organization.
        /// </summary>
        /// <param name="organizationId">Organization id.</param>
        Task DeleteUserRolesAsync(long organizationId);
    }
}