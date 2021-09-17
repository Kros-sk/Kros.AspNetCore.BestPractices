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
        public async Task DeleteColor(int colorValue, long organizationId)
        {
            await _tableStorageManagementService.DeleteValue(organizationId, colorValue);
        }

        /// <inheritdoc />
        public IEnumerable<ColorEntity> GetUsedColors(long organizationId)
            => _tableStorageManagementService.GetAllValuesForPartition(organizationId);

        /// <inheritdoc />
        public async Task SetUsedColor(long organizationId, int colorValue)
        {
            await _tableStorageManagementService.AddRow(organizationId, colorValue);
        }

        ///<inheritdoc/>
        public async Task DeleteAllColors(long organizationId)
        {
            await _tableStorageManagementService.DeleteAllValuesForPartitionKey(organizationId);
        }

        ///<inheritdoc/>
        public int CheckAndGenerateColor(long organizationId, int colorARGBValue, int oldColorARGBValue)
        {
            var colors = GetUsedColors(organizationId);

            if (colorARGBValue == 0)
            {
                colorARGBValue = RandomColor.GetColor(ColorScheme.Random, Luminosity.Bright).ToArgb();
            }
            else
            {
                if (colors.Any(c => c.ColorValue == colorARGBValue.ToString()) &&
                        (oldColorARGBValue != colorARGBValue))
                {
                    return 0;
                }
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
