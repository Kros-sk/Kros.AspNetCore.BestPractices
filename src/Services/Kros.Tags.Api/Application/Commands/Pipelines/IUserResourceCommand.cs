namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Interface, which describe command for changing user resource.
    /// </summary>
    public interface IUserResourceCommand
    {
        /// <summary>
        /// Resource Id.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        long OrganizationId { get; }

        /// <summary>
        /// User Id.
        /// </summary>
        long UserId { get; }
    }
}
