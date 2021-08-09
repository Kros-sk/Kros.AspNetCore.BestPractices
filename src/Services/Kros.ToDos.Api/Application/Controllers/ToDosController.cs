using Kros.AspNetCore;
using Kros.AspNetCore.Authorization;
using Kros.ToDos.Api.Application.Commands;
using Kros.ToDos.Api.Application.Queries;
using Kros.ToDos.Base.Extensions;
using Kros.ToDos.Base.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Application.Controllers
{
    /// <summary>
    /// ToDos controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthorizationHelper.JwtSchemeName)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ToDosController : ApiBaseController
    {
        /// <summary>
        /// Get user ToDos.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">
        /// Forbidden when user doesn't have permission for reading ToDos.
        /// </response>
        /// <returns>ToDos.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>))]
        [Authorize(PoliciesHelper.ReaderAuthPolicyName)]
        public async Task<IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>> Get()
            => await this.SendRequest(new GetAllToDoHeadersQuery(User.GetUserId(), User.GetOrganizationId()));

        /// <summary>
        /// Get ToDo by id.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">Forbidden when user doesn't have permission for ToDo with <paramref name="id"/>.</response>
        /// <response code="404">If ToDo with id <paramref name="id"/> doesn't exist.</response>
        [HttpGet("{id}", Name = nameof(GetToDo))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetToDoQuery.ToDo))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(PoliciesHelper.ReaderAuthPolicyName)]
        public async Task<GetToDoQuery.ToDo> GetToDo(int id)
            => await this.SendRequest(new GetToDoQuery(id, User.GetUserId(), User.GetOrganizationId()));

        /// <summary>
        /// Create new ToDo.
        /// </summary>
        /// <param name="command">Data for creating todo.</param>
        /// <response code="201">Created. ToDo id in body.</response>
        /// <response code="403">Forbidden when user doesn't have permission for creating ToDos.</response>
        /// <returns>ToDo id.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(PoliciesHelper.WriterAuthPolicyName)]
        public async Task<ActionResult> CreateToDo(CreateToDoCommand command)
        {
            command.UserId = User.GetUserId();
            command.OrganizationId = User.GetOrganizationId();

            return await this.SendCreateCommand(command, nameof(GetToDo));
        }

        /// <summary>
        /// Update ToDo.
        /// </summary>
        /// <param name="command">Data for updating todo.</param>
        /// <param name="id">ToDo id.</param>
        /// <response code="403">Forbidden when user doesn't have permission for editing ToDo with <paramref name="id"/>.</response>
        /// <response code="404">If ToDo with id <paramref name="id"/> doesn't exist.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(PoliciesHelper.WriterAuthPolicyName)]
        public async Task<ActionResult> UpdateToDo(int id, UpdateToDoCommand command)
        {
            command.UserId = User.GetUserId();
            command.OrganizationId = User.GetOrganizationId();
            command.Id = id;

            await this.SendRequest(command);

            return Ok();
        }

        /// <summary>
        /// Delete ToDo.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permission for delete ToDo with <paramref name="id"/>.</response>
        /// <response code="404">If ToDo with id <paramref name="id"/> doesn't exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(PoliciesHelper.WriterAuthPolicyName)]
        public async Task<ActionResult> DeleteToDo(int id)
        {
            await this.SendRequest(new DeleteToDoCommand(id, User.GetUserId(), User.GetOrganizationId()));

            return NoContent();
        }

        /// <summary>
        /// Deletes completed ToDos.
        /// </summary>
        /// <response code="204">Deleted.</response>
        /// <response code="403">Forbidden when user doesn't have permission for delete ToDos.</response>
        [HttpDelete("deleteCompleted")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(PoliciesHelper.WriterAuthPolicyName)]
        public async Task<ActionResult> DeleteCompletedToDos()
        {
            await this.SendRequest(new DeleteCompletedToDosCommand(User.GetUserId(), User.GetOrganizationId()));

            return NoContent();
        }

        /// <summary>
        /// Change todo is done state.
        /// </summary>
        /// <param name="id">ToDo id.</param>
        /// <param name="command">New IsDone state.</param>
        /// <response code="200">Ok.</response>
        /// <response code="403">Forbidden when user doesn't have permission for edit ToDo with <paramref name="id"/>.</response>
        /// <response code="404">If ToDo with id <paramref name="id"/> doesn't exist.</response>
        [HttpPut("changeIsDoneState/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(PoliciesHelper.WriterAuthPolicyName)]
        public async Task<ActionResult> ChangeIsDoneState(int id, ChangeIsDoneStateCommand command)
        {
            command.UserId = User.GetUserId();
            command.OrganizationId = User.GetOrganizationId();
            command.Id = id;

            await this.SendRequest(command);

            return Ok();
        }
    }
}
