using Kros.KORM;
using Kros.ToDos.Base.Infrastructure;
using Kros.Utils;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Query handler for User queries.
    /// </summary>
    public class GetUsersQueryHandler :
        IRequestHandler<GetUserQuery, GetUserQuery.User>,
        IRequestHandler<GetUserByEmailQuery, GetUserByEmailQuery.User>,
        IRequestHandler<GetAllUsersQuery, IEnumerable<GetAllUsersQuery.User>>
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public GetUsersQueryHandler(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public Task<GetUserQuery.User> Handle(GetUserQuery request, CancellationToken cancellationToken)
            => Task.FromResult(
                _database.Query<GetUserQuery.User>().SingleOrDefault(t => t.Id == request.UserId));

        /// <inheritdoc />
        public Task<GetUserByEmailQuery.User> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
            => Task.FromResult(
                _database.Query<GetUserByEmailQuery.User>().SingleOrDefault(t => t.Email == request.UserEmail));

        /// <inheritdoc />
        public Task<IEnumerable<GetAllUsersQuery.User>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
            => Task.FromResult(_database.Query<GetAllUsersQuery.User>()
                        .Select("Users.id", "Users.FirstName", "Users.LastName", "Users.Email", "permissions.value as Permissions")
                        .From("Permissions INNER JOIN Users ON (Permissions.UserId = Users.Id) ")
                        .Where($"Permissions.organizationId = {request.OrganizationId} AND Permissions.PermissionKey = {PermissionsHelper.Claims.UserRole} ")
                        .AsEnumerable());
    }
}
