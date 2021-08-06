using Kros.Authorization.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Commands
{
    /// <summary>
    /// Delete user permissions command handler.
    /// </summary>
    public class DeleteUserPermissionsCommandHandler :
        IRequestHandler<DeleteAllPermissionsByOrganizationCommand, Unit>,
        IRequestHandler<DeleteUserPermissionsByOrganizationCommand, Unit>
    {
        /// <summary>
        /// MediatR command handler for command to delete user role.
        /// </summary>
        private readonly IPermissionRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User role repository.</param>
        public DeleteUserPermissionsCommandHandler(IPermissionRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteUserPermissionsByOrganizationCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteUserRolesInOrganizationAsync(request.OrganizationId, request.UserId);

            return Unit.Value;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteAllPermissionsByOrganizationCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAllUserRolesInOrganizationAsync(request.OrganizationId);

            return Unit.Value;
        }
    }
}
