using Kros.Authorization.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Commands
{
    /// <summary>
    /// MediatR command handler for command to create user role.
    /// </summary>
    public class CreatePermissionsCommandHandler : IRequestHandler<CreatePermissionsCommand, string>
    {
        private readonly IPermissionRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User role repository.</param>
        public CreatePermissionsCommandHandler(IPermissionRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<string> Handle(CreatePermissionsCommand request, CancellationToken cancellationToken)
        {
            Permission userRole = request.Adapt<Permission>();

            await _repository.TryUpdatePermissionAsync(userRole);

            return userRole.Value;
        }
    }
}
