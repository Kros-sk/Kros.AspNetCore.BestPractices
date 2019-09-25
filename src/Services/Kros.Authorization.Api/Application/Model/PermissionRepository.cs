using Kros.KORM;
using Kros.KORM.Query;
using Kros.Utils;
using System.Linq;
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
        public async Task TryUpdatePermissionAsync(Permission permission)
        {
            var userPermissions = _database.Query<Permission>().AsDbSet();

            if (_database.Query<Permission>().Any("UserId = @1 AND OrganizationId = @2 AND PermissionKey = @3",
                                                   permission.UserId, permission.OrganizationId, permission.Key))
            {
                userPermissions.Edit(permission);
            }
            else
            {
                userPermissions.Add(permission);
            }

            await userPermissions.CommitChangesAsync();
        }

        ///<inheritdoc />
        public async Task DeleteAllUserRolesInOrganizationAsync(long organizationId)
        {
            await _database.DeleteAsync<Permission>(ur => ur.OrganizationId == organizationId);
        }

        ///<inheritdoc />
        public async Task DeleteUserRolesInOrganizationAsync(long organizationId, long userId)
        {
            await _database.DeleteAsync<Permission>(ur => ur.OrganizationId == organizationId && ur.UserId == userId);
        }
    }
}