using Kros.Utils;
using RandomColorGenerator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Services
{
    /// <summary>
    /// Service providing methods for managing colors for tags.
    /// </summary>
    public class ColorManagementService : IColorManagementService
    {
        private readonly ITableStorageManagementService _tableStorageManagementService;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tableStorageManagementService">Table storage management service.</param>
        public ColorManagementService(ITableStorageManagementService tableStorageManagementService)
        {
            _tableStorageManagementService = Check.NotNull(tableStorageManagementService, nameof(tableStorageManagementService));
        }

        /// <inheritdoc />
        public Task DeleteColor(int colorValue, long organizationId)
            => _tableStorageManagementService.DeleteValue(organizationId, colorValue);

        /// <inheritdoc />
        public IEnumerable<ColorEntity> GetUsedColors(long organizationId)
            => _tableStorageManagementService.GetAllValuesForPartition(organizationId);

        /// <inheritdoc />
        public Task SetUsedColor(long organizationId, int colorValue)
            => _tableStorageManagementService.AddRow(organizationId, colorValue);

        ///<inheritdoc/>
        public Task DeleteAllColors(long organizationId)
            => _tableStorageManagementService.DeleteAllValuesForPartitionKey(organizationId);

        ///<inheritdoc/>
        public int CheckAndGenerateColor(long organizationId, int colorARGBValue, int oldColorARGBValue)
        {
            if ((colorARGBValue == oldColorARGBValue) && (colorARGBValue != 0))
            {
                return colorARGBValue;
            }
            var colors = GetUsedColors(organizationId);

            if (colors.Any(c => c.ColorValue == colorARGBValue.ToString()) && (oldColorARGBValue != colorARGBValue))
            {
                return 0;
            }

            if (!colors.Any(c => c.ColorValue == colorARGBValue.ToString()) && colorARGBValue != 0)
            {
                return colorARGBValue;
            }

            if (colorARGBValue == 0)
            {
                colorARGBValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
            }
            var colorExistsInStorage = colors.Any(c => c.ColorValue == colorARGBValue.ToString() &&
                (oldColorARGBValue != colorARGBValue));

            while (colorExistsInStorage)
            {
                var colorValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
                colorARGBValue = colorValue;
                colorExistsInStorage = colors.Any(c => c.ColorValue == colorARGBValue.ToString());
            }

            return colorARGBValue;
        }
    }
}
