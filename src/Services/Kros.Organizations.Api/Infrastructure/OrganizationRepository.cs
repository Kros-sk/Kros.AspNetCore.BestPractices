using Kros.Organizations.Api.Domain;
using Kros.KORM;
using Kros.Utils;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Infrastructure
{
    /// <summary>
    /// Repository for persistating <see cref="Organization"/>.
    /// </summary>
    public class OrganizationRepository: IOrganizationRepository
    {
        private IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public OrganizationRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task CreateOrganizationAsync(Organization item)
        {
            await _database.AddAsync(item);
        }

        /// <inheritdoc />
        public async Task UpdateOrganizationAsync(Organization item)
        {
            var dbSet = _database
                .Query<Organization>()
                .AsDbSet();

            dbSet.Edit(item);

            await dbSet.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteOrganizationAsync(long id)
        {
            await _database.DeleteAsync<Organization>(or => or.Id == id);
        }
    }
}
