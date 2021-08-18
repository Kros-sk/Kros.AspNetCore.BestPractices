using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Domain
{
    /// <summary>
    /// Interface which describe repository for persistating <see cref="Tag"/>.
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        /// Creates new tag in repository.
        /// </summary>
        /// <param name="tag">Created tag.</param>
        /// <returns></returns>
        Task CreateTagAsync(Tag tag);

        /// <summary>
        /// Updates tag in repository.
        /// </summary>
        /// <param name="tag">Updated tag.</param>
        /// <returns></returns>
        Task UpdateTagAsync(Tag tag);

        /// <summary>
        /// Deletes tag by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Tag Id.</param>
        /// <returns></returns>
        Task DeleteTagAsync(long id);

        /// <summary>
        /// Deletes all tags for organization by <paramref name="organizationId"/>.
        /// </summary>
        /// <param name="organizationId">Organization Id.</param>
        /// <returns></returns>
        Task DeleteAllTagsAsync(long organizationId);
    }
}
