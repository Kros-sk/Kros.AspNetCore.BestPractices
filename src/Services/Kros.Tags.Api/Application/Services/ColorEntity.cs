using Microsoft.Azure.Cosmos.Table;

namespace Kros.Tags.Api.Application.Services
{
    /// <summary>
    /// Row data.
    /// </summary>
    public class ColorEntity : TableEntity
    {
        /// <summary>
        /// Organization Id.
        /// </summary>
        public string OrganizationId { get => PartitionKey; set => PartitionKey = value; }

        /// <summary>
        /// Color value.
        /// </summary>
        public string ColorValue { get => RowKey; set => RowKey = value; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public ColorEntity() { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="partitionKey">Partition key.</param>
        /// <param name="rowKey">Row key.</param>
        public ColorEntity(long partitionKey, int rowKey)
        {
            PartitionKey = partitionKey.ToString();
            RowKey = rowKey.ToString();
        }
    }
}
