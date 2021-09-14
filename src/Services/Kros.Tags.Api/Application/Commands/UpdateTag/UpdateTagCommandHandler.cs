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
            var colors = _colorManagementService.GetUsedColors(request.OrganizationId);
            if (request.ColorARGBValue == 0)
            {
                request.ColorARGBValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
            }
            else
            {
                if (colors.Any(c => c.ColorValue == request.ColorARGBValue.ToString()) &&
                        (request.OldColorARGBValue != request.ColorARGBValue))
                {
                    throw new InvalidOperationException("Color already exist in storage.");
                }
            }
            var colorExistsInStorage = colors.Any(c => c.ColorValue == request.ColorARGBValue.ToString() &&
                (request.OldColorARGBValue != request.ColorARGBValue));

            while (colorExistsInStorage)
            {
                var colorValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
                request.ColorARGBValue = colorValue;
                colorExistsInStorage = colors.Any(c => c.ColorValue == request.ColorARGBValue.ToString());
            }

            await _colorManagementService.DeleteColor(request.OldColorARGBValue, request.OrganizationId);

            await _colorManagementService.SetUsedColor(request.OrganizationId, request.ColorARGBValue);

            var item = request.Adapt<Tag>();
            await _tagRepository.UpdateTagAsync(item);

            return Unit.Value;
        }
    }
}
