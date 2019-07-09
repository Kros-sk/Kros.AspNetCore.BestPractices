using Kros.Authorization.Api.Application.Model;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Commands.UpdatePermissions
{
    /// <summary>
    /// Update user permissions command handler.
    /// </summary>
    public class UpdatePermissionsCommandHandler : IRequestHandler<UpdatePermissionsCommand>
    {
        private readonly IPermissionRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User permissions repository.</param>
        public UpdatePermissionsCommandHandler(IPermissionRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {
            var userPermissions = request.Adapt<Permission>();
            await _repository.TryUpdatePermissionAsync(userPermissions);

            return Unit.Value;
        }
    }
}