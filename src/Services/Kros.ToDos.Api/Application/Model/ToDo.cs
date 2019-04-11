using Kros.KORM.Metadata;
using Kros.KORM.Metadata.Attribute;
using System;

namespace Kros.ToDos.Api.Application.Model
{
    /// <summary>
    /// ToDo model.
    /// </summary>
    [Alias("ToDos")]
    public class ToDo
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Key(autoIncrementMethodType: AutoIncrementMethodType.Custom)]
        public int Id { get; set; }

        /// <summary>
        /// ToDo Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Created?
        /// </summary>
        public DateTime Created { get; set; }
    }
}
