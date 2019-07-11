using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Model
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
    }
}