using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Model
{
    /// <summary>
    /// Interface which describe repository for persistating <see cref="ToDo"/>.
    /// </summary>
    public interface IToDoRepository
    {
        /// <summary>
        /// Create new todo in repository.
        /// </summary>
        /// <param name="toDo">Creating todo.</param>
        Task CreateToDoAsync(ToDo toDo);

        /// <summary>
        /// Update todo in repository.
        /// </summary>
        /// <param name="toDo">Updating todo.</param>
        Task UpdateToDoAsync(ToDo toDo);

        /// <summary>
        /// Delete ToDo  by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        Task DeleteToDoAsync(int id);
    }
}
