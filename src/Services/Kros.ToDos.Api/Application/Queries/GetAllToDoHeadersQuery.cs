using Kros.KORM.Metadata.Attribute;
using MediatR;
using System.Collections.Generic;

namespace Kros.ToDos.Api.Application.Queries
{
    /// <summary>
    /// Get all todo headers.
    /// </summary>
    public class GetAllToDoHeadersQuery : IRequest<IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        public GetAllToDoHeadersQuery(int userId, int organizationId)
        {
            UserId = userId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        public int OrganizationId { get; }

        /// <summary>
        /// ToDo Header
        /// </summary>
        [Alias("ToDos")]
        public class ToDoHeader
        {
            /// <summary>
            /// ToDo Id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// ToDo Name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Is todo marked as done?
            /// </summary>
            public bool IsDone { get; set; }
        }
    }
}
