using Kros.Authorization.Api.Application.Services;
using Kros.Utils;
using MediatR;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Queries.Users
{
    /// <summary>
    /// Handler for user rights queries.
    /// </summary>
    public class UserRightsQueryHandler :
        IRequestHandler<UserAdminRightsQuery, bool>,
        IRequestHandler<UserReaderRightsQuery, bool>,
        IRequestHandler<UserWriterRightsQuery, bool>
    {
        private readonly ClaimsPrincipal _claims;
        private readonly IPermissionService _permissions;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="claims">User claims.</param>
        /// <param name="permissions"><see cref="IPermissionService"/>.</param>
        public UserRightsQueryHandler(ClaimsPrincipal claims, IPermissionService permissions)
        {
            _claims = Check.NotNull(claims, nameof(claims));
            _permissions = Check.NotNull(permissions, nameof(permissions));
        }

        /// <inheritdoc />
        public Task<bool> Handle(UserAdminRightsQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_permissions.IsAdminFromClaims(_claims));

        /// <inheritdoc />
        public Task<bool> Handle(UserReaderRightsQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_permissions.IsReaderFromClaims(_claims));

        /// <inheritdoc />
        public Task<bool> Handle(UserWriterRightsQuery request, CancellationToken cancellationToken)
            => Task.FromResult(_permissions.IsWriterFromClaims(_claims));
    }
}
