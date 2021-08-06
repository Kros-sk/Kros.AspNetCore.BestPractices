using Kros.Authorization.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Commands
{
    /// <summary>
    /// Update user command handler.
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User repository.</param>
        /// <param name="cache">Cache.</param>
        public UpdateUserCommandHandler(IUserRepository repository, IMemoryCache cache)
        {
            _repository = Check.NotNull(repository, nameof(repository));
            _cache = Check.NotNull(cache, nameof(cache));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = request.Adapt<User>();
            await _repository.UpdateUserAsync(user);

            _cache.Remove(request.Email);

            return Unit.Value;
        }
    }
}
