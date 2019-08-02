using Kros.ToDos.Api.Application.Commands.PipeLines;
using MediatR;
using Newtonsoft.Json;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Change is done state command.
    /// </summary>
    public class ChangeIsDoneStateCommand : IRequest<Unit>, IUserResourceCommand
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        [JsonIgnore]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Is todo marked as done?
        /// </summary>
        public bool IsDone { get; set; }
    }
}