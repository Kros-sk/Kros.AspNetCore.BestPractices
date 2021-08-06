using Kros.Authorization.Api.Domain;
using Kros.KORM;
using Kros.Utils;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Model
{
    /// <summary>
    /// Repository for persistating <see cref="User"/>.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public UserRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task CreateUserAsync(User user)
            => _database.AddAsync(user);

        /// <inheritdoc />
        public Task UpdateUserAsync(User user)
            => _database.EditAsync(user);
    }
}
