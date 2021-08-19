using Kros.Tags.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
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

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        public UpdateTagCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var item = request.Adapt<Tag>();
            await _tagRepository.UpdateTagAsync(item);

            return Unit.Value;
        }
    }
}
