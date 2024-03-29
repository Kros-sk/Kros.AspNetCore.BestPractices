﻿using Kros.KORM.Metadata.Attribute;
using Kros.Organizations.Api.Application.Queries.PipeLines;
using Kros.Organizations.Api.Infrastructure;
using MediatR;

namespace Kros.Organizations.Api.Application.Queries
{
    /// <summary>
    /// Get Organization by Id.
    /// </summary>
    public class GetOrganizationQuery : IRequest<GetOrganizationQuery.Organization>, IUserResourceQuery
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization id.</param>
        /// <param name="userId">User id.</param>
        public GetOrganizationQuery(long organizationId, long userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; }

        /// <summary>
        /// Organization
        /// </summary>
        [Alias(DatabaseConfiguration.OrganizationsTableName)]
        public class Organization : IUserResourceQueryResult
        {
            /// <summary>
            /// Organization Id.
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
}
