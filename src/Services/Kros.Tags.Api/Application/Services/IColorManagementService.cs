using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Services
{
    /// <summary>
    /// Interface for color management service.
    /// </summary>
    public interface IColorManagementService
    {

        /// <summary>
        /// Set used color.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="colorValue">Color ARGB value.</param>
        /// <returns></returns>
        Task SetUsedColor(long organizationId, int colorValue);

        /// <summary>
        /// Get all used colors for organization.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns></returns>
        IEnumerable<ColorEntity> GetUsedColors(long organizationId);

        /// <summary>
        /// Delete unused color for tag.
        /// </summary>
        /// <param name="colorValue"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        Task DeleteColor(int colorValue, long organizationId);

        /// <summary>
        /// Delete all colors for organization.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns></returns>
        Task DeleteAllColors(long organizationId);
    }
}
