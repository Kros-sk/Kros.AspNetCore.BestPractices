using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Domain
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
        Task DeleteToDoAsync(long id);

        /// <summary>
        /// Deletes completed Todos.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        Task<IEnumerable<long>> DeleteCompletedToDosAsync(long userId, long organizationId);

        /// <summary>
        /// Change is done state to <paramref name="isDone"/>.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        /// <param name="isDone">New is done state.</param>
        Task ChangeIsDoneState(long id, bool isDone);
    }
}
