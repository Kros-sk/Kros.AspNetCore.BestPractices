using MediatR;

namespace Kros.Authorization.Api.Application.Commands.CreatePermissions
{
    /// <summary>
    /// MediatR command to create user role.
    /// </summary>
    public class CreatePermissionsCommand : IRequest<string>
    {
        /// <summary>
        /// User id.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Organization id.
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// Permission key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Permission value.
        /// </summary>
        public string Value { get; set; }
    }
}
