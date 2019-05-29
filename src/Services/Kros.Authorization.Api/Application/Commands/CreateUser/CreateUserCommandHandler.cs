using Kros.Authorization.Api.Application.Model;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Commands
{
    /// <summary>
    /// Create user command handler.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User repository.</param>
        public CreateUserCommandHandler(IUserRepository repository)
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
