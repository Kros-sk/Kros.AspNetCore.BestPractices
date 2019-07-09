using System.Threading.Tasks;

namespace Kros.Organizations.Api.Domain
{
    /// <summary>
    /// Interface which describe repository for persistating <see cref="Organization"/>.
    /// </summary>
    public interface IOrganizationRepository
    {
        /// <summary>
        /// Create new item in repository.
        /// </summary>
        /// <param name="item">Creating item.</param>
        Task CreateOrganizationAsync(Organization item);

        /// <summary>
        /// Update item in repository.
        /// </summary>
        /// <param name="item">Updating item.</param>
        Task UpdateOrganizationAsync(Organization item);

        /// <summary>
        /// Delete item by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Item id.</param>
        Task DeleteOrganizationAsync(int id);
    }
}
