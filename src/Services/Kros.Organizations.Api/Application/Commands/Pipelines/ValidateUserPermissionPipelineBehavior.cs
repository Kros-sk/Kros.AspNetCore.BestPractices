using Kros.AspNetCore.Exceptions;
using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Commands.Pipelines
{
    /// <summary>
    /// Pipeline behavior for validating if queried resource belong to user.
    /// </summary>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    public class ValidateUserPermissionPipelineBehavior<TRequest> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IUserResourceCommand, IRequest<Unit>
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public ValidateUserPermissionPipelineBehavior(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(
            TRequest request,
            RequestHandlerDelegate<Unit> next,
            CancellationToken cancellationToken)
        {
            var organization = _database.Query<Organization>()
                .FirstOrDefault(uo => uo.Id == request.Id);

            if (organization == null)
            {
                throw new NotFoundException();
            }

            if (organization.UserId != request.UserId)
            {
                throw new ResourceIsForbiddenException(string.Format(Properties.Resources.ForbiddenMessage,
                    request.UserId, typeof(Organization), request.Id));
            }

            return await next();
        }

        [Alias("Organizations")]
        private class Organization
        {
            /// <summary>
            /// Organization Id
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// User Id
            /// </summary>
            public int UserId { get; set; }
        }
    }
}
