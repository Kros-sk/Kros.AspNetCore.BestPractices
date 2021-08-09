using Kros.Authorization.Api.Domain;
using Kros.KORM;
using Kros.Utils;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Model
{
    /// <summary>
    /// Repository for persistating <see cref="Permission"/>.
    /// </summary>
    public class PermissionRepository : IPermissionRepository
    {
        private IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public PermissionRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task TryUpdatePermissionAsync(Permission permission)
            => _database.UpsertAsync(permission);

        ///<inheritdoc />
        public Task DeleteAllUserRolesInOrganizationAsync(long organizationId)
            => _database.DeleteAsync<Permission>(ur => ur.OrganizationId == organizationId);

        ///<inheritdoc />
        public Task DeleteUserRolesInOrganizationAsync(long organizationId, long userId)
        => _database.DeleteAsync<Permission>(ur => ur.OrganizationId == organizationId && ur.UserId == userId);
    }
}