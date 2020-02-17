namespace Kros.Organizations.Api.Domain
{
    /// <summary>
    /// Organization model.
    /// </summary>
    public class Organization
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Organization name
        /// </summary>
        public string OrganizationName { get; set; }

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
