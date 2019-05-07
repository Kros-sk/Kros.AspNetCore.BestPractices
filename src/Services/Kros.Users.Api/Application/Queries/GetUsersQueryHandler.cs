using Kros.KORM;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Users.Api.Application.Queries
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
            => Task.FromResult(_database.Query<GetAllUsersQuery.User>().AsEnumerable());
    }
}
