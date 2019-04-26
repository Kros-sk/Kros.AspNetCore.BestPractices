using MediatR;
using System.Collections.Generic;

namespace Kros.ToDos.Api.Application.Notifications
{
    /// <summary>
    /// ToDos was deleted notification.
    /// </summary>
    public class ToDosDeleted : INotification
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="ids">Deleted ToDo ids.</param>
        /// <param name="userId">User id.</param>
        public ToDosDeleted(IEnumerable<int> ids, int userId)
        {
            Ids = ids;
            UserId = userId;
        }

        /// <summary>
        /// Deleted ToDo ids.
        /// </summary>
        public IEnumerable<int> Ids { get; }

        /// <summary>
        /// User id.
        /// </summary>
        public int UserId { get; }
    }
}
