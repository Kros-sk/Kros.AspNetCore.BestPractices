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
        int Id { get; }

        /// <summary>
        /// User id.
        /// </summary>
        int UserId { get; }
    }
}
