using MediatR;

namespace Kros.ToDos.Api.Application.Notifications
{
    /// <summary>
    /// ToDo was updated notification.
    /// </summary>
    public class ToDoUpdated : INotification
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        /// <param name="userId">User id.</param>
        public ToDoUpdated(int id, int userId)
        {
            Id = id;
            UserId = userId;
        }

        /// <summary>
        /// ToDo id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// User id.
        /// </summary>
        public int UserId { get; }
    }
}
