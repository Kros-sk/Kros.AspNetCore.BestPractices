using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kros.KORM;
using Kros.Utils;
using System.Linq;

namespace Kros.Tags.Api.Application.Queries
{
    /// <summary>
    /// Query handler for tag queries.
    /// </summary>
    public class GetTagsHandler
        : IRequestHandler<GetAllTagsQuery, IEnumerable<GetAllTagsQuery.Tag>>,
        IRequestHandler<GetTagQuery, GetTagQuery.Tag>
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public GetTagsHandler(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task<IEnumerable<GetAllTagsQuery.Tag>> Handle(
            GetAllTagsQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetAllTagsQuery.Tag>()
                .Where($"OrganizationId = {request.OrganizationId}")
                .AsEnumerable());

        /// <inheritdoc />
        public Task<GetTagQuery.Tag> Handle(
            GetTagQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetTagQuery.Tag>()
                .FirstOrDefault(o => o.Id == request.TagId));
    }
}
