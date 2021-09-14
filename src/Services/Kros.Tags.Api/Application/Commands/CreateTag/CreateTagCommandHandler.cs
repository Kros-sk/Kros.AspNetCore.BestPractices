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
    /// <see cref="CreateTagCommand"/> handler.
    /// </summary>
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, long>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IColorManagementService _colorManagementService;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        /// <param name="colorManagementService">Color management service.</param>
        public CreateTagCommandHandler(ITagRepository tagRepository, IColorManagementService colorManagementService)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
            _colorManagementService = Check.NotNull(colorManagementService, nameof(colorManagementService));
        }

        /// <inheritdoc />
        public async Task<long> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var colors = _colorManagementService.GetUsedColors(request.OrganizationId);

            if (request.ColorARGBValue == 0)
            {
                var colorValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
                request.ColorARGBValue = colorValue;
            }
            else
            {
                if (colors.Any(c => c.ColorValue == request.ColorARGBValue.ToString()))
                {
                    throw new InvalidOperationException("Color already exist in storage.");
                }
            }

            var colorExistsInStorage = colors.Any(c => c.ColorValue == request.ColorARGBValue.ToString());

            while (colorExistsInStorage)
            {
                var colorValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
                request.ColorARGBValue = colorValue;
                colorExistsInStorage = colors.Any(c => c.ColorValue == request.ColorARGBValue.ToString());
            }

            await _colorManagementService.SetUsedColor(request.OrganizationId, request.ColorARGBValue);

            var tag = request.Adapt<Tag>();
            await _tagRepository.CreateTagAsync(tag);

            return tag.Id;
        }

    }
}
