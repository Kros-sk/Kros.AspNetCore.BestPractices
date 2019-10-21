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
        int UserId { get; }

        /// <summary>
        /// Organization id.
        /// </summary>
        int OrganizationId { get; }
    }
}
