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
        /// <param name="organizationId">Organization id.</param>
        public ToDosDeleted(IEnumerable<long> ids, long userId, long organizationId)
        {
            Ids = ids;
            UserId = userId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Deleted ToDo ids.
        /// </summary>
        public IEnumerable<long> Ids { get; }

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
