using Kros.KORM;
using Kros.Organizations.Api.Domain;
using Kros.Utils;
using System;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Infrastructure
{
    /// <inheritdoc />
    public class UserOrganizationRepository : IUserOrganizationRepository
    {

        private IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public UserOrganizationRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task AddUserToOrganizationAsync(int organizationId, int userId)
        {
            var dbSet = _database.Query<UserOrganization>().AsDbSet();

            dbSet.Add(new UserOrganization() {
                OrganizationId = organizationId,
                UserId = userId
            });

            await dbSet.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task RemoveUserFromOrganizationAsync(int organizationId, int userId)
        {
            var dbSet = _database.Query<UserOrganization>().AsDbSet();
            dbSet.Delete(new UserOrganization() {
                OrganizationId = organizationId,
                UserId = userId
            });

            await dbSet.CommitChangesAsync();
        }
    }
}
