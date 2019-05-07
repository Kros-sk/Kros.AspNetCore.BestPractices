using MediatR;
using Newtonsoft.Json;

namespace Kros.Users.Api.Application.Commands
{
    /// <summary>
    /// Update user command.
    /// </summary>
    public class UpdateUserCommand : IRequest
    {
        /// <summary>
        /// User id.
        /// </summary>
        [JsonIgnore]
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