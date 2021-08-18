using Kros.AspNetCore.Exceptions;
using Kros.KORM;
using Kros.KORM.Metadata.Attribute;
using Kros.Utils;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Commands.Pipelines
{
    public class ValidateTagPermissionBehavior<TRequest> : IPipelineBehavior<TRequest, Unit>
        where TRequest : IIdCommand
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="database">Database.</param>
        public ValidateTagPermissionBehavior(IDatabase database)
        {
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<Unit> next)
        {
            var tag = _database.Query<Tag>()
                .FirstOrDefault(t => t.Id == request.Id);

            if (tag == null)
            {
                throw new NotFoundException();
            }

            return await next();
        }

        [Alias("Tags")]
        private class Tag
        {
            /// <summary>
            /// Tag Id
            /// </summary>
            public int Id { get; set; }
        }
    }
}
