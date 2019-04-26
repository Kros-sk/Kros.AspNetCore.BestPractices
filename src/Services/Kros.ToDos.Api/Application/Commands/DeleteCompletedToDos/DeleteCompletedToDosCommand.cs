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
        public DeleteCompletedToDosCommand(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
