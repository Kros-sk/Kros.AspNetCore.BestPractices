using Kros.KORM;
using Kros.Tags.Api.Application.Services;
using Kros.Tags.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
using RandomColorGenerator;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Update tag command handler.
    /// </summary>
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IColorManagementService _colorManagementService;
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        /// <param name="colorManagementService">Color management service.</param>
        /// <param name="database">Database.</param>
        public UpdateTagCommandHandler(
            ITagRepository tagRepository,
            IColorManagementService colorManagementService,
            IDatabase database)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
            _colorManagementService = Check.NotNull(colorManagementService, nameof(colorManagementService));
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var generatedColor = _colorManagementService.CheckAndGenerateColor(
                request.OrganizationId, 
                request.ColorARGBValue, 
                request.OldColorARGBValue);
            if (generatedColor == 0)
            {
                throw new InvalidOperationException("Color already exist in storage.");
            }
            else
            {
                request.ColorARGBValue = generatedColor;
            }

            await _colorManagementService.DeleteColor(request.OldColorARGBValue, request.OrganizationId);

            await _colorManagementService.SetUsedColor(request.OrganizationId, request.ColorARGBValue);

            var item = request.Adapt<Tag>();
            await _tagRepository.UpdateTagAsync(item);

            return Unit.Value;
        }
    }
}
