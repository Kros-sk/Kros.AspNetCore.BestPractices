using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
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
        public async Task CreateToDoAsync(ToDo toDo)
        {
            var todos = _database.Query<ToDo>().AsDbSet();

            toDo.Created = DateTimeOffset.Now;
            toDo.LastChange = DateTimeOffset.Now;
            toDo.IsDone = false;

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

        /// <inheritdoc />
        public async Task DeleteToDoAsync(int id)
        {
            var todos = _database.Query<ToDo>().AsDbSet();
            todos.Delete(new ToDo() { Id = id});

            await todos.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteCompletedToDosAsync()
        {
            var dbSet = _database.Query<ToDo>().AsDbSet();
            var todos = _database.Query<ToDo>().Select(t => t.Id).Where(t => t.IsDone);

            dbSet.Delete(todos);

            await dbSet.CommitChangesAsync();
        }

        /// <inheritdoc />
        public async Task ChangeIsDoneState(int id, bool isDone)
        {
            var todos = _database.Query<ToDoIsDoneEdit>().AsDbSet();
            todos.Edit(new ToDoIsDoneEdit() { Id = id, IsDone = isDone });

            await todos.CommitChangesAsync();
        }

        // Dočasne pokiaľ KORM nevie injektovať Created a LastChange
        private static Lazy<string[]> _editColumns
            = new Lazy<string[]>(()
                => typeof(ToDo).GetProperties()
                .Where(p=> p.Name != nameof(ToDo.Created))
                .Select(p=> p.Name)
                .ToArray());


        [Alias("ToDos")]
        private class ToDoIsDoneEdit
        {
            [Key]
            public int Id { get; set; }

            public bool IsDone { get; set; }
        }
    }
}
