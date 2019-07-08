using Kros.KORM.Metadata.Attribute;
using Kros.Utils;
using MediatR;
using System.Collections.Generic;

namespace Kros.Authorization.Api.Application.Queries.Permission
{
    /// <summary>
    /// Get all user permissions.
    /// </summary>
    public class GetAllUserPermissionsQuery : IRequest<IEnumerable<GetAllUserPermissionsQuery.Permission>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        public GetAllUserPermissionsQuery(int userId)
        {
            UserId = Check.NotNull(userId, nameof(userId));
        }

        /// <summary>
        /// User id.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// User permission model.
        /// </summary>
        [Alias("Permissions")]
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