using Kros.KORM.Metadata.Attribute;
using Kros.Tags.Api.Infrastructure;
using MediatR;
using System.Collections.Generic;

namespace Kros.Tags.Api.Application.Queries
{
    /// <summary>
    /// Get all tags.
    /// </summary>
    public class GetAllTagsQuery : IRequest<IEnumerable<GetAllTagsQuery.Tag>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        public GetAllTagsQuery(long organizationId)
        {
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Organization Id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// Tag.
        /// </summary>
        [Alias(DatabaseConfiguration.TagsTableName)]
        public class Tag
        {
            /// <summary>
            /// Tag Id.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Tag name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Organization Id.
            /// </summary>
            public int OrganizationId { get; set; }

            /// <summary>
            /// Description for tag.
            /// </summary>
            public string Description { get; set; }
        }
    }
}
