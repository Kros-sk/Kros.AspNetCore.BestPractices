using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Tags.Api.Application.Commands;
using Kros.Tags.Api.Application.Queries;
using Kros.ToDos.Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Tags.Api.Application.Controllers
{
    /// <summary>
    /// Tags controller.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TagsController : ApiBaseController
    {

        /// <summary>
        /// Get organization tags.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">
        /// Forbidden when user doesn't have permission for reading tags.
        /// </response>
        /// <returns>All tags for organization.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAllTagsQuery.Tag>))]
        public async Task<IEnumerable<GetAllTagsQuery.Tag>> Get()
            => await this.SendRequest(new GetAllTagsQuery(User.GetOrganizationId()));

        /// <summary>
        /// Get tag by its Id.
        /// </summary>
        /// <param name="id">Tag id.</param>
        /// <response code="200">Ok.</response>
        /// <response code="403">Forbidden when user doesn't have permission for tag with <paramref name="id"/>.</response>
        /// <response code="404">If tag with id <paramref name="id"/> doesn't exist.</response>
        /// <returns>Tag.</returns>
        [HttpGet("{id}", Name = nameof(GetTag))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetTagQuery.Tag))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<GetTagQuery.Tag> GetTag(int id)
            => await this.SendRequest(new GetTagQuery(id, User.GetOrganizationId()));

        /// <summary>
        /// Create new tag.
        /// </summary>
        /// <param name="command">Data for creating new tag.</param>
        /// <response code="201">Created. Tag id in body.</response>
        /// <response code="403">Forbidden when user doesn't have permission for creating tag.</response>
        /// <returns>Tag id.</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateTag(CreateTagCommand command)
        {
            command.OrganizationId = User.GetOrganizationId();

            return await this.SendCreateCommand(command, nameof(GetTag));
        }

        /// <summary>
        /// Delete tag by Id.
        /// </summary>
        /// <param name="id">Tag Id.</param>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permission for delete tag with <paramref name="id"/>.</response>
        /// <response code="404">If tag with id <paramref name="id"/> doesn't exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTag(int id)
        {
            await this.SendRequest(new DeleteTagCommand(id, User.GetOrganizationId()));
            return NoContent();
        }

        /// <summary>
        /// Delete all tags.
        /// </summary>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permission for delete tags.</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAllTags()
        {
            await this.SendRequest(new DeleteAllTagsCommand(User.GetOrganizationId()));
            return NoContent();
        }

        /// <summary>
        /// Update tag.
        /// </summary>
        /// <param name="id">Tag Id.</param>
        /// <param name="command">Data for updated tag.</param>
        /// <response code="403">Forbidden when user doesn't have permission for editing tag with <paramref name="id"/>.
        /// </response>
        /// <response code="404">If tag with id <paramref name="id"/> doesn't exist.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateTag(int id, UpdateTagCommand command)
        {
            command.Id = id;
            command.OrganizationId = User.GetOrganizationId();
            await this.SendRequest(command);

            return Ok();
        }
    }
}
