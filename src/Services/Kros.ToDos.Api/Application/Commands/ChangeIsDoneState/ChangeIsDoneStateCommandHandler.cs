using Kros.ToDos.Api.Application.Notifications;
using Kros.ToDos.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Change todo is done state Command Handler.
    /// </summary>
    public class ChangeIsDoneStateCommandHandler : IRequestHandler<ChangeIsDoneStateCommand, Unit>
    {
        private readonly IToDoRepository _repository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        /// <param name="mediator">Mediator for publishing events.</param>
        public ChangeIsDoneStateCommandHandler(IToDoRepository repository, IMediator mediator)
        {
            _repository = Check.NotNull(repository, nameof(repository));
            _mediator = Check.NotNull(mediator, nameof(mediator));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(ChangeIsDoneStateCommand request, CancellationToken cancellationToken)
        {
            await _repository.ChangeIsDoneState(request.Id, request.IsDone);
            await _mediator.Publish(new ToDoUpdated(request.Id, request.UserId));

            return Unit.Value;
        }
    }
}
