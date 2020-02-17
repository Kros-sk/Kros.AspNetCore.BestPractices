namespace Kros.ToDos.Api.Application.Commands.PipeLines
{
    /// <summary>
    /// Interface, which describe command for changing user resource.
    /// </summary>
    public interface IUserResourceCommand
    {
        /// <summary>
        /// Id.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// User id.
        /// </summary>
        long UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        long OrganizationId { get; }
    }
}
