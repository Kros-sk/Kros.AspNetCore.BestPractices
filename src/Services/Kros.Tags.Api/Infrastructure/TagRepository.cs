using Kros.KORM;
using Kros.Tags.Api.Domain;
using Kros.Utils;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Infrastructure
{
    /// <summary>
    /// Repository for persistating <see cref="Tag"/>.
    /// </summary>
    public class TagRepository : ITagRepository
    {
        private IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public TagRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task CreateTagAsync(Tag tag)
            => _database.AddAsync(tag);

        /// <inheritdoc />
        public Task DeleteAllTagsAsync(long organizationId)
            => _database.DeleteAsync<Tag>(t => t.OrganizationId == organizationId);

        /// <inheritdoc />
        public Task DeleteTagAsync(long id)
        => _database.DeleteAsync<Tag>(t => t.Id == id);

        /// <inheritdoc />
        public Task UpdateTagAsync(Tag tag)
            => _database.EditAsync(tag);
    }
}
