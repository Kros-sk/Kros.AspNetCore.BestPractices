using MediatR;

namespace Kros.Authorization.Api.Application.Commands.DeletePermissions
{
    /// <summary>
    /// MediatR command to delete user role by organization.
    /// </summary>
    public class DeleteUserPermissionsByOrganizationCommand : IRequest<Unit>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization id.</param>
        /// <param name="userId">User id.</param>
        public DeleteUserPermissionsByOrganizationCommand(long organizationId, long userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// User id.
        /// </summary>
        public long UserId { get; }

    }
}

