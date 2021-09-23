using Kros.KORM.Metadata.Attribute;
using Kros.Tags.Api.Infrastructure;
using MediatR;

namespace Kros.Tags.Api.Application.Queries
{
    /// <summary>
    /// Get tag.
    /// </summary>
    public class GetTagQuery : IRequest<GetTagQuery.Tag>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="tagId">Tag Id.</param>
        /// <param name="organizationId">Organization Id.</param>
        public GetTagQuery(long tagId, long organizationId)
        {
            TagId = tagId;
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Organization Id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// Tag Id.
        /// </summary>
        public long TagId { get; }

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

            /// <summary>
            /// User Id.
            /// </summary>
            public long UserId { get; set; }

            /// <summary>
            /// ARGB value for color.
            /// </summary>
            public int ColorARGBValue { get; set; }
        }
    }
}
