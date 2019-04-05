using Kros.KORM.Metadata.Attribute;
using MediatR;
using System;
using System.Collections.Generic;

namespace Kros.ToDos.Api.Application.Queries
{
    /// <summary>
    /// Get ToDo by Id.
    /// </summary>
    public class GetToDoQuery : IRequest<GetToDoQuery.ToDo>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="todoId">ToDo id.</param>
        public GetToDoQuery(int todoId, int userId)
        {
            ToDoId = todoId;
            UserId = userId;
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
        /// ToDo Header
        /// </summary>
        [Alias("ToDos")]
        public class ToDo
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
            public DateTime Created { get; set; }

            /// <summary>
            /// User Id.
            /// </summary>
            public int UserId { get; set; }
        }
    }
}
