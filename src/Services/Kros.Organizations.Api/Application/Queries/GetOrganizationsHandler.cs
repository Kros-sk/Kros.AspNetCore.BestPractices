using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kros.KORM;
using Kros.Utils;
using System.Linq;

namespace Kros.Organizations.Api.Application.Queries
{
    /// <summary>
    /// Query handler for Organization queries.
    /// </summary>
    public class GetOrganizationsQueryHandler
        : IRequestHandler<GetAllOrganizationsQuery, IEnumerable<GetAllOrganizationsQuery.Organization>>,
        IRequestHandler<GetOrganizationQuery, GetOrganizationQuery.Organization>
    {
        private readonly IDatabase _database;

        private const string FROM = "Organizations INNER JOIN UserOrganization ON (Organizations.Id = UserOrganization.OrganizationId)";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public GetOrganizationsQueryHandler(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task<IEnumerable<GetAllOrganizationsQuery.Organization>> Handle(
            GetAllOrganizationsQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetAllOrganizationsQuery.Organization>()
                .From(FROM)
                .Where("UserOrganization.UserId = @1", request.UserId).AsEnumerable());

        /// <inheritdoc />
        public Task<GetOrganizationQuery.Organization> Handle(
            GetOrganizationQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetOrganizationQuery.Organization>()
                .From(FROM)
                .Where("UserOrganization.UserId = @1 AND UserOrganization.OrganizationId = @2", request.UserId, request.OrganizationId)
                .FirstOrDefault());
    }
}
