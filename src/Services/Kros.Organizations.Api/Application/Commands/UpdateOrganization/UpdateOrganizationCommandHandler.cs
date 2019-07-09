using Kros.Organizations.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Update Organization Command Handler.
    /// </summary>
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand>
    {
        private readonly IOrganizationRepository _repository;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="repository">Organization repository.</param>
        public UpdateOrganizationCommandHandler (IOrganizationRepository repository)
        {
            _repository = Check.NotNull(repository, nameof(repository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            var item = request.Adapt<Organization>();
            await _repository.UpdateOrganizationAsync(item);

            return Unit.Value;
        }
    }
}
