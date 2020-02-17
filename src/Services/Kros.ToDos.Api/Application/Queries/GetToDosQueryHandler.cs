using Kros.KORM;
using Kros.ToDos.Api.Application.Notifications;
using Kros.Utils;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Queries
{
    /// <summary>
    /// Query handler for ToDo queries.
    /// </summary>
    public class GetToDosQueryHandler :
        IRequestHandler<GetAllToDoHeadersQuery, IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>>,
        IRequestHandler<GetToDoQuery, GetToDoQuery.ToDo>,
        INotificationHandler<ToDoUpdated>,
        INotificationHandler<ToDosDeleted>
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
                GetKey<GetAllToDoHeadersQuery.ToDoHeader>(request.UserId, request.OrganizationId),
                () => _database.Query<GetAllToDoHeadersQuery.ToDoHeader>()
                               .Where($"UserId = {request.UserId} AND OrganizationId = {request.OrganizationId}")
                               .AsEnumerable(),
                _options.Value);

        /// <inheritdoc />
        public Task<GetToDoQuery.ToDo> Handle(GetToDoQuery request, CancellationToken cancellationToken)
            => _cache.GetAndSetAsync(
                GetKey<GetToDoQuery.ToDo>(request.ToDoId),
                () => _database.Query<GetToDoQuery.ToDo>().First(t => t.Id == request.ToDoId),
                _options.Value);

        /// <inheritdoc />
        public Task Handle(ToDoUpdated notification, CancellationToken cancellationToken)
        {
            _cache.RemoveAsync(GetKey<GetAllToDoHeadersQuery.ToDoHeader>(notification.UserId, notification.OrganizationId));
            _cache.RemoveAsync(GetKey<GetToDoQuery.ToDo>(notification.Id));

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Handle(ToDosDeleted notification, CancellationToken cancellationToken)
        {
            _cache.RemoveAsync(GetKey<GetAllToDoHeadersQuery.ToDoHeader>(notification.UserId, notification.OrganizationId));
            foreach (var id in notification.Ids)
            {
                _cache.RemoveAsync(GetKey<GetAllToDoHeadersQuery.ToDoHeader>(id));
            }

            return Task.CompletedTask;
        }

        private string GetKey<T>(params long[] ids)
            => $"{typeof(T).Name}:{string.Join(",", ids.Select(id => id.ToString()))}";
    }
}
