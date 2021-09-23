using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Services
{
    /// <summary>
    /// Interface for Azure table storage management.
    /// </summary>
    public interface ITableStorageManagementService
    {
        /// <summary>
        /// Add row to table storage.
        /// </summary>
        /// <param name="partitionKey">Partition key.</param>
        /// <param name="rowKey">Row key.</param>
        Task AddRow(long partitionKey, int rowKey);

        /// <summary>
        /// Get all values for specified partition.
        /// </summary>
        /// <param name="partitionKey">Partition key.</param>
        /// <returns>All values from table.</returns>
        IEnumerable<ColorEntity> GetAllValuesForPartition(long partitionKey);

        /// <summary>
        /// Delete row from table storage.
        /// </summary>
        /// <param name="rowKey">Row key.</param>
        /// <param name="partitionKey">Partition key</param>
        Task DeleteValue(long partitionKey, int rowKey);

        /// <summary>
        /// Delete all rows for partition key.
        /// </summary>
        /// <param name="partitionKey">Partition key.</param>
        Task DeleteAllValuesForPartitionKey(long partitionKey);
    }
}
