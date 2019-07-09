using Kros.KORM.Metadata.Attribute;
using MediatR;
using System.Collections.Generic;

namespace Kros.Organizations.Api.Application.Queries
{
    /// <summary>
    /// Get all OrganizationPlural.
    /// </summary>
    public class GetAllOrganizationsQuery : IRequest<IEnumerable<GetAllOrganizationsQuery.Organization>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public GetAllOrganizationsQuery(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Organization
        /// </summary>
        [Alias("Organizations")]
        public class Organization
        {
            /// <summary>
            /// Organization Id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Company name
            /// </summary>
            public string CompanyName { get; set; }

            /// <summary>
            /// Business Id
            /// </summary>
            public string BusinessId { get; set; }
        }
    }
}
