using Kros.KORM.Metadata;
using Kros.KORM.Metadata.Attribute;

namespace Kros.Organizations.Api.Domain
{
    /// <summary>
    /// Organization model.
    /// </summary>
    [Alias("Organizations")]
    public class Organization
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Key(autoIncrementMethodType: AutoIncrementMethodType.Custom)]
        public int Id { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Business Id
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// Address - Street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Address - Street Number
        /// </summary>
        public string StreetNumber { get; set; }

        /// <summary>
        /// Address - City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address - Zip Code
        /// </summary>
        public string ZipCode { get; set; }

    }
}
