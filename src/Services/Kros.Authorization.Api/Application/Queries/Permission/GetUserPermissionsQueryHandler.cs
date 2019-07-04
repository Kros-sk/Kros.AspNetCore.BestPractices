using Kros.KORM;
using Kros.Utils;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Queries.Permission
{
    /// <summary>
    /// Query handler for user permissions queries.
    /// </summary>
    public class GetUserPermissionsQueryHandler :
        IRequestHandler<GetUserPermissionsForOrganizationQuery, IEnumerable<GetUserPermissionsForOrganizationQuery.Permission>>,
        IRequestHandler<GetAllUserPermissionsQuery, IEnumerable<GetAllUserPermissionsQuery.Permission>>
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public GetUserPermissionsQueryHandler(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task<IEnumerable<GetUserPermissionsForOrganizationQuery.Permission>> Handle(
            GetUserPermissionsForOrganizationQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetUserPermissionsForOrganizationQuery.Permission>()
                                        .Where(p => p.UserId == request.UserId && p.OrganizationId == request.OrganizationId)
                                        .AsEnumerable());

        /// <inheritdoc />
        public Task<IEnumerable<GetAllUserPermissionsQuery.Permission>> Handle(
            GetAllUserPermissionsQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetAllUserPermissionsQuery.Permission>()
                                        .Where(p => p.UserId == request.UserId)
                                        .AsEnumerable());
    }
}