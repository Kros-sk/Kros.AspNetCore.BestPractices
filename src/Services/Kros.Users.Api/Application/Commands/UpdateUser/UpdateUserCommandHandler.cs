using Kros.Users.Api.Application.Model;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Users.Api.Application.Commands.UpdateUser
{
    /// <summary>
    /// Update user command handler.
    /// </summary>
    public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User repository.</param>
        public UpdateUserCommandHandler(IUserRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();
            await _repository.UpdateUserAsync(user);

            return Unit.Value;
        }
    }
}
