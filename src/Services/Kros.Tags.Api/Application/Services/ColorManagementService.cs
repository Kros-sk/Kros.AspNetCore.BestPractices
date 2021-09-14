using Kros.Utils;
using System.Collections.Generic;
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
    }
}
