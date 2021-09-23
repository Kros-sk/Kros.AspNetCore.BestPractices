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
        /// <param name="organizationId">Organization Id.</param>
        /// <param name="colorValue">Color value.</param>
        public ColorEntity(long organizationId, int colorValue)
        {
            PartitionKey = organizationId.ToString();
            RowKey = colorValue.ToString();
        }
    }
}
