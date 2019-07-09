using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Domain
{

    /// <summary>
    /// Manage connection between Organizations and Users
    /// </summary>
    public interface IUserOrganizationRepository
    {

        /// <summary>
        /// Add user to organization (new connection between organization and user)
        /// </summary>
        /// <param name="organizationId">Organization to which the user should be added</param>
        /// <param name="userId">User Id</param>
        Task AddUserToOrganizationAsync(int organizationId, int userId);

        /// <summary>
        /// Remove user from organization (delete connection between organization and user)
        /// </summary>
        /// <param name="organizationId">Organization from which the user should be removed</param>
        /// <param name="userId">User Id</param>
        Task RemoveUserFromOrganizationAsync(int organizationId, int userId);

    }
}
