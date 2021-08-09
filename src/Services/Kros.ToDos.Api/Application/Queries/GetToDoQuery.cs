using Kros.KORM.Metadata.Attribute;
using Kros.ToDos.Api.Application.Queries.PipeLines;
using Kros.ToDos.Api.Infrastructure;
using MediatR;
using System;

namespace Kros.ToDos.Api.Application.Queries
{
    /// <summary>
    /// Get ToDo by Id.
    /// </summary>
    public class GetToDoQuery : IRequest<GetToDoQuery.ToDo>, IUserResourceQuery
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="todoId">ToDo id.</param>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        public GetToDoQuery(long todoId, long userId, long organizationId)
        {
            ToDoId = todoId;
            UserId = userId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// ToDo id.
        /// </summary>
        public long ToDoId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// ToDo Header
        /// </summary>
        [Alias(DatabaseConfiguration.ToDosTableName)]
        public class ToDo : IUserResourceQueryResult
        {
            /// <summary>
            /// ToDo Id.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// ToDo Name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Created.
            /// </summary>
            public DateTimeOffset Created { get; set; }

            /// <summary>
            /// Date time of last change.
            /// </summary>
            public DateTimeOffset LastChange { get; set; }

            /// <summary>
            /// User Id.
            /// </summary>
            public long UserId { get; set; }

            /// <summary>
            /// Organization id.
            /// </summary>
            public long OrganizationId { get; set; }

            /// <summary>
            /// Is todo marked as done?
            /// </summary>
            public bool IsDone { get; set; }
        }
    }
}
