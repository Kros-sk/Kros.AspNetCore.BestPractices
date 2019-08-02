using Kros.ToDos.Api.Application.Notifications;
using Kros.ToDos.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Delete ToDo Command Handler.
    /// </summary>
    public class DeleteToDoCommandHandler : IRequestHandler<DeleteToDoCommand, Unit>
    {
        private readonly IToDoRepository _repository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        /// <param name="mediator">Mediator for publishing events.</param>
        public DeleteToDoCommandHandler(IToDoRepository repository, IMediator mediator)
        {
            _repository = Check.NotNull(repository, nameof(repository));
            _mediator = Check.NotNull(mediator, nameof(mediator));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteToDoCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteToDoAsync(request.Id);
            await _mediator.Publish(new ToDoUpdated(request.Id, request.UserId, request.OrganizationId));

            return Unit.Value;
        }
    }
}
