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
        private readonly IUserOrganizationRepository _userOrganizationRepository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationRepository">Organization repository.</param>
        /// <param name="userOrganizationRepository">UserOrganization repository managing connection between Users and Organizations.</param>
        public DeleteOrganizationCommandHandler(IOrganizationRepository organizationRepository, IUserOrganizationRepository userOrganizationRepository)
        {
            _organizationRepository = Check.NotNull(organizationRepository, nameof(organizationRepository));
            _userOrganizationRepository = Check.NotNull(userOrganizationRepository, nameof(userOrganizationRepository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            await _userOrganizationRepository.RemoveUserFromOrganizationAsync(request.Id, request.UserId);
            await _organizationRepository.DeleteOrganizationAsync(request.Id);

            return Unit.Value;
        }
    }
}
