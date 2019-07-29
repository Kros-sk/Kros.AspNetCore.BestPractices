
using Kros.Authorization.Api.Application.Model;
using Kros.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Authorization.Api.Application.Commands.DeletePermissions
{
    /// <summary>
    /// Delete all user permissions command handler.
    /// </summary>
    public class DeleteAllPermissionsCommandHandler : IRequestHandler<DeleteAllPermissionsByCompanyCommand, Unit>
    {
        /// <summary>
        /// MediatR command handler for command to delete user role.
        /// </summary>
        private readonly IPermissionRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">User role repository.</param>
        public DeleteAllPermissionsCommandHandler(IPermissionRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteAllPermissionsByCompanyCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAllUserRolesInCompanyAsync(request.CompanyId);

            return Unit.Value;
        }

    }
}
