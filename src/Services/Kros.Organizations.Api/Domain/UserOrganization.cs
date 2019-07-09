using Kros.KORM.Metadata.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Domain
{
    /// <summary>
    /// User - Organization model.
    /// </summary>
    [Alias("UserOrganization")]
    public class UserOrganization
    {

        /// <summary>
        /// User Id
        /// </summary>
        [Key("PK_UserOrganization", 1)]
        public int UserId { get; set; }

        /// <summary>
        /// Organization Id
        /// </summary>
        [Key("PK_UserOrganization", 2)]
        public int OrganizationId { get; set; }

    }
}
