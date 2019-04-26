using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Model
{
    /// <summary>
    /// Interface which describe repository for persistating <see cref="ToDo"/>.
    /// </summary>
    public interface IToDoRepository
    {
        /// <summary>
        /// Creates new todo in repository.
        /// </summary>
        /// <param name="toDo">Creating todo.</param>
        Task CreateToDoAsync(ToDo toDo);

        /// <summary>
        /// Updates todo in repository.
        /// </summary>
        /// <param name="toDo">Updating todo.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Deletes ToDo by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        Task DeleteToDoAsync(int id);

        /// <summary>
        /// Deletes completed Todos.
        /// </summary>
        Task DeleteCompletedToDosAsync();

        /// <summary>
        /// Change is done state to <paramref name="isDone"/>.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        /// <param name="isDone">New is done state.</param>
        Task ChangeIsDoneState(int id, bool isDone);
    }
}
