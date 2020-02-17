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
        /// <param name="organizationId">Organization id.</param>
        public ToDoUpdated(long id, long userId, long organizationId)
        {
            Id = id;
            UserId = userId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// ToDo id.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// User id.
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; }
    }
}
