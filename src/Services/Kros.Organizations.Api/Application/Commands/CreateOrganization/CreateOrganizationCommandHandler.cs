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
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, int>
    {
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationRepository">Organization repository.</param>
        public CreateOrganizationCommandHandler (IOrganizationRepository organizationRepository)
        {
            _organizationRepository = Check.NotNull(organizationRepository, nameof(organizationRepository));
        }

        /// <inheritdoc />
        public async Task<int> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var item = request.Adapt<Organization>();

            await _organizationRepository.CreateOrganizationAsync(item);

            return item.Id;
        }
    }
}
