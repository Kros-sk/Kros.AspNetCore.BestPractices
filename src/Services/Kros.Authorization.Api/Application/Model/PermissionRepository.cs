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
        public async Task DeleteAllUserRolesInCompanyAsync(long companyId)
        {
            IDbSet<Permission> dbSet = _database.Query<Permission>().AsDbSet();
            dbSet.Delete(dbSet.Where(ur => ur.OrganizationId == companyId));
            await dbSet.CommitChangesAsync();
        }

        ///<inheritdoc />
        public async Task DeleteUserRolesInCompanyAsync(long companyId, long userId)
        {
            IDbSet<Permission> dbSet = _database.Query<Permission>().AsDbSet();
            dbSet.Delete(dbSet.Where(ur => ur.OrganizationId == companyId && ur.UserId == userId));
            await dbSet.CommitChangesAsync();
        }
    }
}