using Kros.Tags.Api.Application.Services;
using Kros.Tags.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Delete tag command handler.
    /// </summary>
    public class DeleteTagCommandHandler
        : IRequestHandler<DeleteTagCommand, Unit>,
        IRequestHandler<DeleteAllTagsCommand, Unit>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IColorManagementService _colorManagementService;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        /// <param name="colorManagementService">Color management service.</param>
        public DeleteTagCommandHandler(ITagRepository tagRepository, IColorManagementService colorManagementService)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
            _colorManagementService = Check.NotNull(colorManagementService, nameof(colorManagementService));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            await _tagRepository.DeleteTagAsync(request.Id, request.OrganizationId);
            await _colorManagementService.DeleteColor(request.ColorARGBValue, request.OrganizationId);
            return Unit.Value;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteAllTagsCommand request, CancellationToken cancellationToken)
        {
            await _colorManagementService.DeleteAllColors(request.OrganizationId);
            await _tagRepository.DeleteAllTagsAsync(request.OrganizationId);
            return Unit.Value;
        }
    }
}
