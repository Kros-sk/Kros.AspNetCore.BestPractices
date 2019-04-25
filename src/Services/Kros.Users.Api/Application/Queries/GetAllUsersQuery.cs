using Kros.KORM.Metadata.Attribute;
using MediatR;
using System.Collections.Generic;

namespace Kros.Users.Api.Application.Queries
{
    /// <summary>
    /// Get all users.
    /// </summary>
    public class GetAllUsersQuery : IRequest<IEnumerable<GetAllUsersQuery.User>>
    {
        /// <summary>
        /// User Header
        /// </summary>
        [Alias("Users")]
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
            /// Is user admin?
            /// </summary>
            public bool IsAdmin { get; set; }
        }
    }
}
