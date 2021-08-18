using MediatR;
using Newtonsoft.Json;

namespace Kros.Tags.Api.Application.Commands.DeleteTag
{
    /// <summary>
    /// Delete all tags command.
    /// </summary>
    public class DeleteAllTagsCommand : IRequest<Unit>
    {

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        public DeleteAllTagsCommand(long organizationId)
        {
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
