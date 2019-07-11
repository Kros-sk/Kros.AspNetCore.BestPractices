namespace Kros.ToDos.Api.Application.Queries.PipeLines
{
    /// <summary>
    /// Interface, which describe query for user resource.
    /// </summary>
    public interface IUserResourceQuery
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
