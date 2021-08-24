namespace Kros.Tags.Api.Application.Commands
{
    /// <summary>
    /// Interface which describe command for changing tag resource.
    /// </summary>
    public interface ITagManagementCommand
    {
        /// <summary>
        /// Tag Id.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Organization Id.
        /// </summary>
        long OrganizationId { get; }
    }
}
