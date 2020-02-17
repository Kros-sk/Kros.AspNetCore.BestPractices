namespace Kros.ToDos.Api.Application.Queries.PipeLines
{
    /// <summary>
    /// Interface, which describe user resource result.
    /// </summary>
    public interface IUserResourceQueryResult
    {
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
