//using Kros.Tags.Api.Application.Services;
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

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        public DeleteTagCommandHandler(ITagRepository tagRepository)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            await _tagRepository.DeleteTagAsync(request.Id, request.OrganizationId);
            return Unit.Value;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteAllTagsCommand request, CancellationToken cancellationToken)
        {
            await _tagRepository.DeleteAllTagsAsync(request.OrganizationId);
            return Unit.Value;
        }
    }
}
