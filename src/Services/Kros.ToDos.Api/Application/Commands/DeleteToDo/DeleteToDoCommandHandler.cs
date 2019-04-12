using Kros.ToDos.Api.Application.Model;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Create ToDo Command Handler.
    /// </summary>
    public class DeleteToDoCommandHandler: IRequestHandler<DeleteToDoCommand, Unit>
    {
        private readonly IToDoRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        public DeleteToDoCommandHandler(IToDoRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteToDoAsync(request.Id);

            return Unit.Value;
        }
    }
}
