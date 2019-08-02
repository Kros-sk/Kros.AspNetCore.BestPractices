using MediatR;
using Newtonsoft.Json;

namespace Kros.ToDos.Api.Application.Commands
{
    /// <summary>
    /// Delete completed ToDos command.
    /// </summary>
    public class DeleteCompletedToDosCommand : IRequest<Unit>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        public DeleteCompletedToDosCommand(int userId, int organizationId)
        {
            UserId = userId;
            OrganizationId = organizationId;
        }

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
