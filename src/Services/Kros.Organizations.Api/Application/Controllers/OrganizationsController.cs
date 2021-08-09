using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.Organizations.Api.Application.Commands;
using Kros.Organizations.Api.Application.Queries;
using Kros.ToDos.Base.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Controllers
{
    /// <summary>
    /// Organizations controller.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class OrganizationsController : ApiBaseController
    {
        /// <summary>
        /// Get all organizations.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <returns>All user's organizations.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAllOrganizationsQuery.Organization>))]
        public async Task<IEnumerable<GetAllOrganizationsQuery.Organization>> Get()
            => await this.SendRequest(new GetAllOrganizationsQuery(User.GetUserId()));

        /// <summary>
        /// Get organization by id.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">
        /// Forbidden when user don't have permission for organization with <paramref name="id"/>.
        /// </response>
        /// <response code="404">If organization with id <paramref name="id"/> doesn't exist.</response>
        /// <returns>Organization.</returns>
        [HttpGet("{id}", Name = nameof(GetOrganization))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrganizationQuery.Organization))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(PoliciesHelper.ReaderAuthPolicyName)]
        public async Task<GetOrganizationQuery.Organization> GetOrganization(int id)
            => await this.SendRequest(new GetOrganizationQuery(id, User.GetUserId()));

        /// <summary>
        /// Create new organization.
        /// </summary>
        /// <param name="command">Data for creating organization.</param>
        /// <response code="201">Created. Organization id in body.</response>
        /// <returns>
        /// Organization id.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateOrganization(CreateOrganizationCommand command)
        {
            command.UserId = User.GetUserId();
            return await this.SendCreateCommand(command, nameof(GetOrganization));
        }

        /// <summary>
        /// Update organization.
        /// </summary>
        /// <param name="command">Data for updating organization.</param>
        /// <param name="id">Organization id.</param>
        /// <response code="200">Updated.</response>
        /// <response code="403">Forbidden when user doesn't have permission for updating organization with <paramref name="id"/>.</response>
        /// <response code="404">If organization with id <paramref name="id"/> doesn't exist.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrganization(int id, UpdateOrganizationCommand command)
        {
            command.Id = id;
            command.UserId = User.GetUserId();

            await this.SendRequest(command);

            return Ok();
        }

        /// <summary>
        /// Delete organization.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permission for deleting organization with <paramref name="id"/>.</response>
        /// <response code="404">If organization with id <paramref name="id"/> doesn't exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(PoliciesHelper.OwnerAuthPolicyName)]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            await this.SendRequest(new DeleteOrganizationCommand(id, User.GetUserId()));

            return NoContent();
        }
    }
}
