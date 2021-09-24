using MediatR;
using Newtonsoft.Json;

namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Update tag command.
    /// </summary>
    public class UpdateTagCommand : IRequest, ITagManagementCommand, IUserResourceCommand
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
        [JsonIgnore]
        public long OrganizationId { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public long UserId { get; set; }

        /// <summary>
        /// ARGB value for color.
        /// </summary>
        public int ColorARGBValue { get; set; }
    }
}
