using Kros.KORM;
using Kros.Utils;
using System;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Model
{
    /// <summary>
    /// Repository for persistating <see cref="ToDo"/>.
    /// </summary>
    public class ToDoRepository : IToDoRepository
    {
        private IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database</param>
        public ToDoRepository(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task CreateToDoAsync(ToDo toDo)
        {
            var todos = _database.Query<ToDo>().AsDbSet();

            toDo.Created = DateTimeOffset.Now;
            todos.Add(toDo);

            await todos.CommitChangesAsync();
        }
    }
}
