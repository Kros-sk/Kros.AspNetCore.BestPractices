using Kros.KORM;
using Kros.Utils;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Queries
{
    /// <summary>
    /// Query handler for tag queries.
    /// </summary>
    public class GetTagsHandler
        : IRequestHandler<GetAllTagsQuery, IEnumerable<GetAllTagsQuery.Tag>>,
        IRequestHandler<GetTagQuery, GetTagQuery.Tag>,
        IRequestHandler<GetAllColorsQuery, IEnumerable<GetAllColorsQuery.Color>>
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

        public Task<IEnumerable<GetAllColorsQuery.Color>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetAllColorsQuery.Color>()
                .Where($"OrganizationId = {request.OrganizationId}")
                .AsEnumerable());
    }
}
