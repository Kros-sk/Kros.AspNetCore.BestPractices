using Kros.KORM;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Queries
{
    /// <summary>
    /// Query handler for ToDo queries.
    /// </summary>
    public class GetToDosQueryHandler : IRequestHandler<GetAllToDoHeadersQuery, IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>>
    {
        private readonly IDatabase _database;

        public GetToDosQueryHandler(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task<IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>> Handle(
            GetAllToDoHeadersQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(
                _database.Query<GetAllToDoHeadersQuery.ToDoHeader>().Where(t => t.UserId == request.UserId).AsEnumerable());
    }
}
