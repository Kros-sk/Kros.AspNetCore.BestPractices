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

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationRepository">Organization repository.</param>
        public DeleteOrganizationCommandHandler(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = Check.NotNull(organizationRepository, nameof(organizationRepository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            await _organizationRepository.DeleteOrganizationAsync(request.Id);

            return Unit.Value;
        }
    }
}
