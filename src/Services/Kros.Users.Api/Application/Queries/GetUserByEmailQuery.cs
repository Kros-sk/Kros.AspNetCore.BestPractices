using Kros.KORM.Metadata.Attribute;
using Kros.Utils;
using MediatR;

namespace Kros.Users.Api.Application.Queries
{
    /// <summary>
    /// Get user by email.
    /// </summary>
    public class GetUserByEmailQuery : IRequest<GetUserByEmailQuery.User>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="userEmail">User email.</param>
        public GetUserByEmailQuery(string userEmail)
        {
            UserEmail = Check.NotNullOrWhiteSpace(userEmail, nameof(userEmail));
        }

        /// <summary>
        /// User email.
        /// </summary>
        public string UserEmail { get; }

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
