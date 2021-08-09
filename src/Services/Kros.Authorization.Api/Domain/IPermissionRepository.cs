using System.Threading.Tasks;

namespace Kros.Authorization.Api.Domain
{
    /// <summary>
    /// Interface which describe repository for persistating <see cref="Permission"/>.
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Updates user permission in repository or creates it if specified permission does not exist.
        /// </summary>
        /// <param name="permission">Updating user permission.</param>
        Task TryUpdatePermissionAsync(Permission permission);

        /// <summary>
        /// Deletes all user roles by <paramref name="organizationId"/>.
        /// </summary>
        /// <param name="organizationId"></param>
        Task DeleteAllUserRolesInOrganizationAsync(long organizationId);

        /// <summary>
        /// Deletes all user roles by <paramref name="organizationId"/> and <paramref name="userId"/>.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="userId"></param>
        Task DeleteUserRolesInOrganizationAsync(long organizationId, long userId);
    }
}