using Kros.Authorization.Api.Infrastructure;
using Kros.KORM.Metadata.Attribute;
using MediatR;
using System.Collections.Generic;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Get all users.
    /// </summary>
    public class GetAllUsersQuery : IRequest<IEnumerable<GetAllUsersQuery.User>>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="organizationId">Organization id.</param>
        public GetAllUsersQuery(long organizationId)
        {
            OrganizationId = organizationId;
        }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; }

        /// <summary>
        /// User Header
        /// </summary>
        [Alias(DatabaseConfiguration.UsersTableName)]
        public class User
        {
            /// <summary>
            /// User's Id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// First name.
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            /// Last name.
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// User's email.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// User permissions.
            /// </summary>
            public string Permissions { get; set; }
        }
    }
}
