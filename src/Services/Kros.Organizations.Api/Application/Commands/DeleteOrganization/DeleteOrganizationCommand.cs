using Kros.Organizations.Api.Application.Commands.Pipelines;
using MediatR;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Delete Organization command.
    /// </summary>
    public class DeleteOrganizationCommand : IRequest<Unit>, IUserResourceCommand
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization id.</param>
        /// <param name="userId">User id.</param>
        public DeleteOrganizationCommand(long organizationId, long userId)
        {
            Id = organizationId;
            UserId = userId;
        }

        /// <summary>
        /// Organization Id.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        public long UserId { get; }
    }
}