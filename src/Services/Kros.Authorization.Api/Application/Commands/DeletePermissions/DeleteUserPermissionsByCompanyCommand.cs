using MediatR;

namespace Kros.Authorization.Api.Application.Commands.DeletePermissions
{
    /// <summary>
    /// MediatR command to delete user role by company.
    /// </summary>
    public class DeleteUserPermissionsByCompanyCommand : IRequest<Unit>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="companyId">Company id.</param>
        /// <param name="userId">User id.</param>
        public DeleteUserPermissionsByCompanyCommand(long companyId, long userId)
        {
            CompanyId = companyId;
            UserId = userId;
        }

        /// <summary>
        /// Company id.
        /// </summary>
        public long CompanyId { get; }

        /// <summary>
        /// User id.
        /// </summary>
        public long UserId { get; }

    }
}

