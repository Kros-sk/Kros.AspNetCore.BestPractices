using Kros.AspNetCore.Exceptions;
using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Commands.PipeLines
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
            var toDo = _database.Query<ToDo>().FirstOrDefault(t => t.Id == request.Id);

            if (toDo == null)
            {
                throw new NotFoundException();
            }

            if (toDo.UserId != request.UserId || toDo.OrganizationId != request.OrganizationId)
            {
                throw new ResourceIsForbiddenException(string.Format(Properties.Resources.ForbiddenMessage,
                    request.UserId, typeof(ToDo), request.Id));
            }

            return await next(); ;
        }

        [Alias("ToDos")]
        private class ToDo
        {
            public int Id { get; set; }

            public int UserId { get; set; }

            public int OrganizationId { get; set; }
        }
    }
}
