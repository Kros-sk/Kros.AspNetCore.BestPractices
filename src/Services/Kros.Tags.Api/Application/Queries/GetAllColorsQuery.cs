using Kros.KORM.Metadata.Attribute;
using Kros.Tags.Api.Infrastructure;
using MediatR;
using System.Collections.Generic;

namespace Kros.Tags.Api.Application.Queries
{
    /// <summary>
    /// Query for getting all colors
    /// </summary>
    public class GetAllColorsQuery : IRequest<IEnumerable<GetAllColorsQuery.Color>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        public GetAllColorsQuery(long organizationId)
        {
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Organization Id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// Color.
        /// </summary>
        [Alias(DatabaseConfiguration.TagsTableName)]
        public class Color
        {
            /// <summary>
            /// ARGB value for color.
            /// </summary>
            public int ColorARGBValue { get; set; }

            /// <summary>
            /// Organization Id.
            /// </summary>
            public int OrganizationId { get; set; }
        }
    }
}
