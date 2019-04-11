using Kros.ToDos.Api.Application.Model;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Update ToDo Command Handler.
    /// </summary>
    public class UpdateToDoCommandHandler: IRequestHandler<UpdateToDoCommand>
    {
        private readonly IToDoRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        public UpdateToDoCommandHandler(IToDoRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
        {
            var toDo = request.Adapt<ToDo>();
            await _repository.UpdateToDoAsync(toDo);

            return Unit.Value;
        }
    }
}
