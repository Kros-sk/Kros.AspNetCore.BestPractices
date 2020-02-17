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
        public GetAllOrganizationsQuery(long userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Organization
        /// </summary>
        [Alias("Organizations")]
        public class Organization
        {
            /// <summary>
            /// Organization Id.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// User id
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
        }
    }
}
