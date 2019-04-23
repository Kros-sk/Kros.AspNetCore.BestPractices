using Kros.Users.Api.Application.Model;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Users.Api.Application.Commands
{
    /// <summary>
    /// Create ToDo Command Handler.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly UserRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">ToDo repository.</param>
        public CreateUserCommandHandler(UserRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();
            await _repository.CreateUserAsync(user);

            return user.Id;
        }
    }
}
