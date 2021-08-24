using System;

namespace Kros.Tags.Api.Domain
{
    /// <summary>
    /// Tag model.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Tag name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id of organization.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Description for tag.
        /// </summary>
        public string Description { get; set; }
    }
}
