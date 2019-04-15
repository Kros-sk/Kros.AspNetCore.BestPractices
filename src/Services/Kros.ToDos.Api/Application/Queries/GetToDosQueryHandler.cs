using Kros.KORM;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System;
using Microsoft.Extensions.Options;

namespace Kros.ToDos.Api.Application.Queries
{
    /// <summary>
    /// Query handler for ToDo queries.
    /// </summary>
    public class GetToDosQueryHandler
        : IRequestHandler<GetAllToDoHeadersQuery, IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>>,
        IRequestHandler<GetToDoQuery, GetToDoQuery.ToDo>
    {
        private readonly IDatabase _database;
        private readonly IDistributedCache _cache;
        private readonly IOptions<DistributedCacheEntryOptions> _options;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="cache">Cache.</param>
        /// <param name="options">Caching option.</param>
        public GetToDosQueryHandler(
            IDatabase database,
            IDistributedCache cache,
            IOptions<DistributedCacheEntryOptions> options)
        {
            _database = Check.NotNull(database, nameof(database));
            _cache = Check.NotNull(cache, nameof(cache));
            _options = Check.NotNull(options, nameof(options));
        }

        /// <inheritdoc />
        public Task<IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>> Handle(
            GetAllToDoHeadersQuery request,
            CancellationToken cancellationToken)
            => _cache.GetAndSetAsync(
                GetKey<GetAllToDoHeadersQuery.ToDoHeader>(request.UserId),
                () => _database.Query<GetAllToDoHeadersQuery.ToDoHeader>().Where($"UserId = {request.UserId}").AsEnumerable(),
                _options.Value);

        /// <inheritdoc />
        public Task<GetToDoQuery.ToDo> Handle(GetToDoQuery request, CancellationToken cancellationToken)
            => _cache.GetAndSetAsync(
                GetKey<GetToDoQuery.ToDo>(request.ToDoId),
                () => _database.Query<GetToDoQuery.ToDo>().First(t => t.Id == request.ToDoId),
                _options.Value);

        private string GetKey<T>(int id)
            => $"{typeof(T).Name}:{id}";
    }
}
