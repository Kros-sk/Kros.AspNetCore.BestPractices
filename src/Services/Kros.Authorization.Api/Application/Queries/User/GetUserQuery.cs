using Kros.Authorization.Api.Infrastructure;
using Kros.KORM.Metadata.Attribute;
using MediatR;

namespace Kros.Authorization.Api.Application.Queries
{
    /// <summary>
    /// Get user by email.
    /// </summary>
    public class GetUserQuery : IRequest<GetUserQuery.User>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userId">User id.</param>
        public GetUserQuery(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// User email.
        /// </summary>
        public int UserId { get; }

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
        }
    }
}
