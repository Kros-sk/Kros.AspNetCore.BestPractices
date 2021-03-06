﻿using Kros.ToDos.Api.Application.Notifications;
using Kros.ToDos.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Delete completed ToDos Command Handler.
    /// </summary>
    public class DeleteCompletedToDosCommandHandler : IRequestHandler<DeleteCompletedToDosCommand, Unit>
    {
        private readonly IToDoRepository _repository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        /// <param name="mediator">Mediator for publishing events.</param>
        public DeleteCompletedToDosCommandHandler(IToDoRepository repository, IMediator mediator)
        {
            _repository = Check.NotNull(repository, nameof(repository));
            _mediator = Check.NotNull(mediator, nameof(mediator));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteCompletedToDosCommand request, CancellationToken cancellationToken)
        {
            var todoIds = await _repository.DeleteCompletedToDosAsync(request.UserId, request.OrganizationId);
            await _mediator.Publish(new ToDosDeleted(todoIds, request.UserId, request.OrganizationId));

            return Unit.Value;
        }
    }
}
