using Kros.ToDos.Api.Application.Model;
using Kros.ToDos.Api.Application.Notifications;
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
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        /// <param name="mediator">Mediator for publishing events.</param>
        public UpdateToDoCommandHandler(IToDoRepository repository, IMediator mediator)
        {
            _repository = Check.NotNull(repository, nameof(repository));
            _mediator = Check.NotNull(mediator, nameof(mediator));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateToDoCommand request, CancellationToken cancellationToken)
        {
            var toDo = request.Adapt<ToDo>();
            await _repository.UpdateToDoAsync(toDo);
            await _mediator.Publish(new ToDoUpdated(toDo.Id, request.UserId));

            return Unit.Value;
        }
    }
}
