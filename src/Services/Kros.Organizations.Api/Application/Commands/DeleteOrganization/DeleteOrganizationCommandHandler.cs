using Kros.Organizations.Api.Application.Services;
using Kros.Organizations.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Create Organization Command Handler.
    /// </summary>
    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, Unit>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRoleService _userRoleService;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationRepository">Organization repository.</param>
        /// <param name="userRoleService"><see cref="IUserRoleService"/></param>
        public DeleteOrganizationCommandHandler(
            IOrganizationRepository organizationRepository,
            IUserRoleService userRoleService)
        {
            _organizationRepository = Check.NotNull(organizationRepository, nameof(organizationRepository));
            _userRoleService = Check.NotNull(userRoleService, nameof(userRoleService));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            await _organizationRepository.DeleteOrganizationAsync(request.Id);
            await _userRoleService.DeleteUserRolesAsync(request.Id);

            return Unit.Value;
        }
    }
}
