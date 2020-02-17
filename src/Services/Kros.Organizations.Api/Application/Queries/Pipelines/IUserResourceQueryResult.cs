namespace Kros.Organizations.Api.Application.Queries.PipeLines
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
    }
}
