using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
using Kros.ToDos.Api.Domain;
using Kros.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Infrastructure
{
    /// <summary>
    /// Repository for persistating <see cref="ToDo"/>.
    /// </summary>
    public class ToDoRepository : IToDoRepository
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public ToDoRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task CreateToDoAsync(ToDo toDo)
        {
            toDo.Created = DateTimeProvider.Now;
            toDo.LastChange = DateTimeProvider.Now;
            toDo.IsDone = false;

            return _database.AddAsync(toDo);
        }

        /// <inheritdoc />
        public Task UpdateToDoAsync(ToDo toDo)
        {
            toDo.LastChange = DateTimeProvider.Now;
            return _database.EditAsync(toDo, default, _editColumns.Value);
        }

        /// <inheritdoc />
        public Task DeleteToDoAsync(long id)
            => _database.DeleteAsync<ToDo>(id);

        /// <inheritdoc />
        public async Task<IEnumerable<long>> DeleteCompletedToDosAsync(long userId, long organizationId)
        {
            using (var transaction = _database.BeginTransaction())
            {
                try
                {
                    var todoIds = _database
                        .Query<long>()
                        .Sql($"SELECT Id FROM ToDos " +
                             $"WHERE (UserId = {userId} AND OrganizationId = {organizationId}) AND (IsDone = 1)")
                        .ToList();
                    await _database.ExecuteNonQueryAsync($"DELETE FROM ToDos " +
                                                         $"WHERE (UserId = {userId} AND OrganizationId = {organizationId}) " +
                                                          "AND (IsDone = 1)");

                    transaction.Commit();

                    return todoIds;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public Task ChangeIsDoneState(long id, bool isDone)
            => _database.EditAsync(new ToDoIsDoneEdit() { Id = id, IsDone = isDone }, default, nameof(ToDoIsDoneEdit.Id), nameof(ToDoIsDoneEdit.IsDone));

        // Dočasne pokiaľ KORM nevie injektovať Created a LastChange
        private static readonly Lazy<string[]> _editColumns
            = new Lazy<string[]>(()
                => typeof(ToDo).GetProperties()
                .Where(p => p.Name != nameof(ToDo.Created))
                .Select(p => p.Name)
                .ToArray());

        [Alias(DatabaseConfiguration.ToDosTableName)]
        private class ToDoIsDoneEdit
        {
            [Key]
            public long Id { get; set; }

            public bool IsDone { get; set; }
        }
    }
}
