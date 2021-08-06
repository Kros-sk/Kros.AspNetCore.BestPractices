using Kros.KORM;
using Kros.Organizations.Api.Domain;
using Kros.Utils;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Infrastructure
{
    /// <summary>
    /// Repository for persistating <see cref="Organization"/>.
    /// </summary>
    public class OrganizationRepository : IOrganizationRepository
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
        public Task CreateOrganizationAsync(Organization item)
            => _database.AddAsync(item);

        /// <inheritdoc />
        public Task UpdateOrganizationAsync(Organization item)
            => _database.EditAsync(item);

        /// <inheritdoc />
        public Task DeleteOrganizationAsync(long id)
            => _database.DeleteAsync<Organization>(or => or.Id == id);
    }
}
