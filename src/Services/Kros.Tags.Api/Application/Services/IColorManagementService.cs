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
        Task SetUsedColor(long organizationId, int colorValue);

        /// <summary>
        /// Get all used colors for organization.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns>Used colors.</returns>
        IEnumerable<ColorEntity> GetUsedColors(long organizationId);

        /// <summary>
        /// Delete unused color for tag.
        /// </summary>
        /// <param name="colorValue"></param>
        /// <param name="organizationId"></param>
        Task DeleteColor(int colorValue, long organizationId);

        /// <summary>
        /// Delete all colors for organization.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        Task DeleteAllColors(long organizationId);

        /// <summary>
        /// Check existence of color in organization if color is set. If not generate color for tag in organization.
        /// <remark>Not set is 0.</remark>
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="colorARGBValue">ARGB value of color.</param>
        /// <param name="oldColorARGBValue">Old ARGB value. Used when tag is updated.</param>
        /// <returns>Generated color. Return 0 if user want to set color that exists in organization.</returns>
        int CheckAndGenerateColor(long organizationId, int colorARGBValue, int oldColorARGBValue);
    }
}
