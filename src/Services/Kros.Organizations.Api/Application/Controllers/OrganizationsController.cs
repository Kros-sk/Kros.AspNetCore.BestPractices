using Kros.AspNetCore.Authorization;
using Kros.Organizations.Api.Application.Commands;
using Kros.Organizations.Api.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.Organizations.Api.Application.Controllers
{
    /// <summary>
    /// Organizations controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    public class OrganizationsController : ControllerBase
    {
        /// <summary>
        /// Get Organizations.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAllOrganizationsQuery.Organization>))]
        public async Task<IEnumerable<GetAllOrganizationsQuery.Organization>> Get()
            => await this.SendRequest(new GetAllOrganizationsQuery(User.GetUserId()));

        /// <summary>
        /// Get Organization by id.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">
        /// Forbidden when user don't have permission for Organization with <paramref name="id"/>.
        /// </response>
        /// <response code="404">If ToDo with id <paramref name="id"/> doesn't exist.</response>
        [HttpGet("{id}", Name = nameof(GetOrganization))]
        [ProducesResponseType(200, Type = typeof(GetOrganizationQuery.Organization))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<GetOrganizationQuery.Organization> GetOrganization(int id)
            => await this.SendRequest(new GetOrganizationQuery(id, User.GetUserId()));

        /// <summary>
        /// Create new Organization.
        /// </summary>
        /// <param name="command">Data for creating Organization.</param>
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
        /// Update Organization.
        /// </summary>
        /// <param name="command">Data for creating Organization.</param>
        /// <param name="id">Organization id.</param>
        /// <response code="403">Forbidden when user don't have permission for Organization with <paramref name="id"/>.</response>
        /// <response code="404">If Organization with id <paramref name="id"/> doesn't exist.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOrganization(int id, UpdateOrganizationCommand command)
        {
            command.Id = id;
            command.UserId = User.GetUserId();

            await this.SendRequest(command);

            return Ok();
        }

        /// <summary>
        /// Delete Organization.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <response code="403">Forbidden when user don't have permission for Organization with <paramref name="id"/>.</response>
        /// <response code="404">If Organization with id <paramref name="id"/> doesn't exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOrganization(int id)
        {
            await this.SendRequest(new DeleteOrganizationCommand(id, User.GetUserId()));

            return Ok();
        }
    }
}
