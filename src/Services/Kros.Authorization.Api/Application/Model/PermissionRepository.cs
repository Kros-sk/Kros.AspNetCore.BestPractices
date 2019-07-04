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
        public async Task TryUpdatePermissionAsync(Permission permission)
        {
            var userPermissions = _database.Query<Permission>().AsDbSet();

            if (_database.Query<Permission>().Any("UserId = {0} AND OrganizationId = {1} AND PermissionKey = {2}",
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
    }
}