using Kros.ToDos.Api.Application.Commands.PipeLines;
using MediatR;
using Newtonsoft.Json;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Delete ToDo command.
    /// </summary>
    public class DeleteToDoCommand : IRequest<Unit>, IUserResourceCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        public DeleteToDoCommand(int id, int userId, int organizationId)
        {
            Id = id;
            UserId = userId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        [JsonIgnore]
        public int OrganizationId { get; }
    }
}