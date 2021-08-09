using Kros.Authorization.Api.Infrastructure;
using Kros.KORM.Metadata.Attribute;
using Kros.Utils;
using MediatR;
using System.Collections.Generic;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Get user permissions for organization.
    /// </summary>
    public class GetUserPermissionsForOrganizationQuery : IRequest<IEnumerable<GetUserPermissionsForOrganizationQuery.Permission>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="organizationId">Organization id.</param>
        public GetUserPermissionsForOrganizationQuery(int userId, int organizationId)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            OrganizationId = Check.NotNull(organizationId, nameof(organizationId));
        }

        /// <summary>
        /// User id.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        public int OrganizationId { get; }

        /// <summary>
        /// User permission model.
        /// </summary>
        [Alias(DatabaseConfiguration.PermissionsTableName)]
        public class Permission
        {
            /// <summary>
            /// User id.
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// Organization id.
            /// </summary>
            public int OrganizationId { get; set; }

            /// <summary>
            /// Permission key.
            /// </summary>
            [Alias("PermissionKey")]
            public string Key { get; set; }

            /// <summary>
            /// Permission value.
            /// </summary>
            public string Value { get; set; }
        }
    }
}