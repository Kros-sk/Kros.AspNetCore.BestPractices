using Kros.KORM;
using Kros.Utils;
using System;
using System.Linq;
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
            toDo.LastChange = DateTimeOffset.Now;
            todos.Add(toDo);

            await todos.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateToDoAsync(ToDo toDo)
        {
            var todos = _database
                .Query<ToDo>()
                .Select(_editColumns.Value)
                .AsDbSet();

            toDo.LastChange = DateTimeOffset.Now;
            todos.Edit(toDo);

            await todos.CommitChangesAsync();
        }

        //Dočasne pokia KORM nevie injektovať Created a LastChange
        private static Lazy<string[]> _editColumns
            = new Lazy<string[]>(()
                => typeof(ToDo).GetProperties()
                .Where(p=> p.Name != nameof(ToDo.Created))
                .Select(p=> p.Name)
                .ToArray());
    }
}
