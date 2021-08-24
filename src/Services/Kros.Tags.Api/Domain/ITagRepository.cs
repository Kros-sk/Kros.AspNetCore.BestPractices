using System.Threading.Tasks;

namespace Kros.Tags.Api.Domain
{
    /// <summary>
    /// Interface which describes repository for persisting <see cref="Tag"/>.
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Creates new tag in repository.
        /// </summary>
        /// <param name="tag">Created tag.</param>
        Task CreateTagAsync(Tag tag);

        /// <summary>
        /// Updates tag in repository.
        /// </summary>
        /// <param name="tag">Updated tag.</param>
        Task UpdateTagAsync(Tag tag);

        /// <summary>
        /// Deletes tag by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Tag Id.</param>
        /// <param name="organizationId">Organization Id.</param>
        Task DeleteTagAsync(long id, long organizationId);

        /// <summary>
        /// Deletes all tags for organization by <paramref name="organizationId"/>.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        Task DeleteAllTagsAsync(long organizationId);
    }
}
