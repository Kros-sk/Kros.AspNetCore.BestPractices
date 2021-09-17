using Kros.AspNetCore.Exceptions;
using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
using Kros.Tags.Api.Infrastructure;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Pipeline behavior for validating if tag belongs to user.
    /// </summary>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    public class ValidateUserPermissionBehavior<TRequest> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IUserResourceCommand
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public ValidateUserPermissionBehavior(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        ///<inheritdoc/>
        public async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
        {
            var tag = _database.Query<Tag>()
                .FirstOrDefault(t => t.Id == request.Id && t.OrganizationId == request.OrganizationId);

            if (tag == null)
            {
                throw new NotFoundException();
            }

            if (tag.UserId != request.UserId)
            {
                throw new ResourceIsForbiddenException(string.Format(Properties.Resources.ForbiddenMessage,
                    request.UserId, typeof(Tag), request.Id));
            }

            return await next();
        }

        [Alias(DatabaseConfiguration.TagsTableName)]
        private class Tag
        {
            /// <summary>
            /// Tag Id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// User Id.
            /// </summary>
            public int OrganizationId { get; set; }

            /// <summary>
            /// User Id.
            /// </summary>
            public int UserId { get; set; }
        }
    }
}
