using Kros.Tags.Api.Application.Commands;
using MediatR;
using Newtonsoft.Json;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Delete tag command.
    /// </summary>
    public class DeleteTagCommand : IRequest<Unit>, ITagManagementCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagId">Tag id.</param>
        /// <param name="organizationId">Organization Id.</param>
        public DeleteTagCommand(long tagId, long organizationId)
        {
            Id = tagId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Id.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        [JsonIgnore]
        public long OrganizationId { get; }
    }
}
