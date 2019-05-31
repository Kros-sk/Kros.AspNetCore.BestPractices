using Kros.KORM;
using Kros.Utils;
using System.Threading.Tasks;

namespace Kros.Users.Api.Application.Model
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
        public async Task CreateUserAsync(User user)
        {
            var users = _database.Query<User>().AsDbSet();
            users.Add(user);
            await users.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(User user)
        {
            var users = _database.Query<User>().AsDbSet();
            users.Edit(user);
            await users.CommitChangesAsync();
        }
    }
}
