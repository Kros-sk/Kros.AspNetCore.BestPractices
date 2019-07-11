using Kros.KORM.Metadata.Attribute;
using Kros.ToDos.Api.Application.Queries.PipeLines;
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
        public GetToDoQuery(int todoId, int userId, int organizationId)
        {
            ToDoId = todoId;
            UserId = userId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// ToDo id.
        /// </summary>
        public int ToDoId { get; set; }

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
        public class ToDo : IUserResourceQueryResult
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
            public int UserId { get; set; }

            /// <summary>
            /// Organization id.
            /// </summary>
            public int OrganizationId { get; set; }

            /// <summary>
            /// Is todo marked as done?
            /// </summary>
            public bool IsDone { get; set; }
        }
    }
}
