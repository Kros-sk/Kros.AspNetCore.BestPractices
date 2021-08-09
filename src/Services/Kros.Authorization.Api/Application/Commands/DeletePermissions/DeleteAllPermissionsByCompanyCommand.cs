using MediatR;

namespace Kros.Authorization.Api.Application.Commands
{
    /// <summary>
    /// MediatR command to delete user role by organization.
    /// </summary>
    public class DeleteAllPermissionsByOrganizationCommand : IRequest<Unit>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization id.</param>
        public DeleteAllPermissionsByOrganizationCommand(long organizationId)
        {
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; }
    }
}
