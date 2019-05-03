using Kros.KORM.Metadata;
using Kros.KORM.Metadata.Attribute;

namespace Kros.Users.Api.Application.Model
{
    /// <summary>
    /// User model.
    /// </summary>
    [Alias("Users")]
    public class User
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Key(autoIncrementMethodType: AutoIncrementMethodType.Custom)]
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
