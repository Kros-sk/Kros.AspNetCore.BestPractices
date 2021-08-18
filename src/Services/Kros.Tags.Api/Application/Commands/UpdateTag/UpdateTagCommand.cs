using Kros.Tags.Api.Application.Commands.Pipelines;
using MediatR;
using Newtonsoft.Json;

namespace Kros.Tags.Api.Application.Commands.UpdateTag
{
    /// <summary>
    /// Update tag command.
    /// </summary>
    public class UpdateTagCommand : IRequest, IIdCommand
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonIgnore]
        public long Id { get; set; }

        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description for tag.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Organization Id
        /// </summary>
        public long OrganizationId { get; set; }
    }
}
