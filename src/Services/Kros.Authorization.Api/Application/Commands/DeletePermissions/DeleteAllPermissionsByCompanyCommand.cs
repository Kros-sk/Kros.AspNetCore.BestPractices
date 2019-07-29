using MediatR;

namespace Kros.Authorization.Api.Application.Commands.DeletePermissions
{
    /// <summary>
    /// MediatR command to delete user role by company.
    /// </summary>
    public class DeleteAllPermissionsByCompanyCommand : IRequest<Unit>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="companyId">Company id.</param>
        public DeleteAllPermissionsByCompanyCommand(long companyId)
        {
            CompanyId = companyId;
        }

        /// <summary>
        /// Company id.
        /// </summary>
        public long CompanyId { get; }

    }
}
