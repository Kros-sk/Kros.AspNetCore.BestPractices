using Kros.KORM;
using Kros.Utils;
using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Kros.AspNetCore.Exceptions;
using System;
using Kros.KORM.Metadata.Attribute;

namespace Kros.ToDos.Api.Application.Commands.PipeLines
{
    /// <summary>
    /// Pipeline behavior for validating if queried resource belong to user.
    /// </summary>
    /// <typeparam name="TRequest">Type of request.</typeparam>
    public class ValidateUserPermissionPipelineBehavior<TRequest> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IUserResourceCommand
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
            CancellationToken cancellationToken,
            RequestHandlerDelegate<Unit> next)
        {
            var toDo = _database.Query<ToDo>().FirstOrDefault(t => t.Id == request.Id);

            if (toDo != null && toDo.UserId != request.UserId)
            {
                throw new ResourceIsForbiddenException(String.Format(Properties.Resources.ForbiddenMessage,
                    request.UserId, typeof(ToDo), request.Id));
            }

            return await next(); ;
        }

        [Alias("ToDos")]
        private class ToDo
        {
            public int Id { get; set; }

            public int UserId { get; set; }
        }
    }
}
