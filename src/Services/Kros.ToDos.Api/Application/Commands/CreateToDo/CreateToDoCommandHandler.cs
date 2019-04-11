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
    public class CreateToDoCommandHandler: IRequestHandler<CreateToDoCommand, int>
    {
        private readonly IToDoRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        public CreateToDoCommandHandler(IToDoRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<int> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            var toDo = request.Adapt<ToDo>();
            await _repository.CreateToDoAsync(toDo);

            return toDo.Id;
        }
    }
}
