using MediatR;

namespace Kros.Authorization.Api.Application.Commands.UpdatePermissions
{
    /// <summary>
    /// Update user permissions command.
    /// </summary>
    public class UpdatePermissionsCommand : IRequest
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
        public string Key { get; set; }

        /// <summary>
        /// Permission value.
        /// </summary>
        public string Value { get; set; }
    }
}