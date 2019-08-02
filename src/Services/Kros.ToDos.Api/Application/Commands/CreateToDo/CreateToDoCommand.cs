using MediatR;
using Newtonsoft.Json;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Create ToDo command.
    /// </summary>
    public class CreateToDoCommand : IRequest<int>
    {
        /// <summary>
        /// ToDo Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Organization id.
        /// </summary>
        [JsonIgnore]
        public int OrganizationId { get; set; }
    }
}