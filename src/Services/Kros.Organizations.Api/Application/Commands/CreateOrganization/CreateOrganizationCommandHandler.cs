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
        private readonly IUserOrganizationRepository _userOrganizationRepository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationRepository">Organization repository.</param>
        /// <param name="userOrganizationRepository">UserOrganization repository managing connection between Users and Organizations.</param>
        public CreateOrganizationCommandHandler (IOrganizationRepository organizationRepository, IUserOrganizationRepository userOrganizationRepository)
        {
            _organizationRepository = Check.NotNull(organizationRepository, nameof(organizationRepository));
            _userOrganizationRepository = Check.NotNull(userOrganizationRepository, nameof(userOrganizationRepository));
        }

        /// <inheritdoc />
        public async Task<int> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var item = request.Adapt<Organization>();
            int userId = request.UserId;

            await _organizationRepository.CreateOrganizationAsync(item);
            await _userOrganizationRepository.AddUserToOrganizationAsync(item.Id, userId);

            return item.Id;
        }
    }
}
