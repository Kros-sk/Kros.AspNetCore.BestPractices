//using Kros.Tags.Api.Application.Services;
using Kros.Tags.Api.Domain;
using Kros.Utils;
using Mapster;
using MediatR;
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

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        public CreateTagCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
        }

        /// <inheritdoc />
        public async Task<long> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = request.Adapt<Tag>();
            await _tagRepository.CreateTagAsync(tag);

            return tag.Id;
        }

    }
}
