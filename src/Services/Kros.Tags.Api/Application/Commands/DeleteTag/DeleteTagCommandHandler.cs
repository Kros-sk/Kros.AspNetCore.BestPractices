using Kros.KORM;
using Kros.Tags.Api.Application.Services;
using Kros.Tags.Api.Domain;
using Kros.Utils;
using MediatR;
using System.Linq;
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
        private readonly IDatabase _database;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagRepository">Tag repository.</param>
        /// <param name="colorManagementService">Color management service.</param>
        /// <param name="database">Database.</param>
        public DeleteTagCommandHandler(
            ITagRepository tagRepository,
            IColorManagementService colorManagementService,
            IDatabase database)
        {
            _tagRepository = Check.NotNull(tagRepository, nameof(tagRepository));
            _colorManagementService = Check.NotNull(colorManagementService, nameof(colorManagementService));
            _database = Check.NotNull(database, nameof(database));
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var deletedTag = Task.FromResult(_database.Query<Tag>()
                .FirstOrDefault(o => o.Id == request.Id));
            if (deletedTag != null)
            {
                await _colorManagementService.DeleteColor(deletedTag.Result.ColorARGBValue, request.OrganizationId);
                await _tagRepository.DeleteTagAsync(request.Id, request.OrganizationId);
            }
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
