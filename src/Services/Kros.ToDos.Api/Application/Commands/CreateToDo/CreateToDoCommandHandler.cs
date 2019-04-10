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
        /// <inheritdoc />
        public async Task<int> Handle(CreateToDoCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(1);
        }
    }
}
