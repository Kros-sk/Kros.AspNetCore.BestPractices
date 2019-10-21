using MediatR;
using Newtonsoft.Json;

namespace Kros.Organizations.Api.Application.Commands
{
    /// <summary>
    /// Create Organization command.
    /// </summary>
    public class CreateOrganizationCommand : IRequest<int>
    {
        /// <summary>
        /// User Id.
        /// </summary>
        [JsonIgnore]
        public int UserId { get; set; }

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