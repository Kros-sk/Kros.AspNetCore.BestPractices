using Kros.Organizations.Api.Application.Services;
using Kros.Organizations.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Create Organization Command Handler.
    /// </summary>
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, long>
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRoleService _userRoleService;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationRepository">Organization repository.</param>
        /// <param name="userRoleService"><see cref="IUserRoleService"/></param>
        public CreateOrganizationCommandHandler(
            IOrganizationRepository organizationRepository,
            IUserRoleService userRoleService)
        {
            _organizationRepository = Check.NotNull(organizationRepository, nameof(organizationRepository));
            _userRoleService = Check.NotNull(userRoleService, nameof(userRoleService));
        }

        /// <inheritdoc />
        public async Task<long> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var organization = request.Adapt<Organization>();

            await _organizationRepository.CreateOrganizationAsync(organization);
            await _userRoleService.CreateOwnerRoleAsync(organization.UserId, organization.Id);

            return organization.Id;
        }
    }
}
