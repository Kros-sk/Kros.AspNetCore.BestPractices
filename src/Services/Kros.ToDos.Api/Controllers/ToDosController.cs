using Kros.ToDos.Api.Application.Commands;
using Kros.ToDos.Api.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Controllers
{
    /// <summary>
    /// ToDos controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        /// <summary>
        /// Get user ToDos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>))]
        public async Task<IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>> Get()
            => await this.SendRequest(new GetAllToDoHeadersQuery(1));

        /// <summary>
        /// Get ToDo by id.
        /// </summary>
        /// <response code="200">Ok.</response>
        /// <response code="403">Forbidden when user don't have permission for ToDo with <paramref name="id"/>.</response>
        /// <response code="404">If ToDo with id <paramref name="id"/> doesn't exist.</response>
        [HttpGet("{id}", Name = nameof(GetToDo))]
        [ProducesResponseType(200, Type = typeof(GetToDoQuery.ToDo))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<GetToDoQuery.ToDo> GetToDo(int id)
            => await this.SendRequest(new GetToDoQuery(id, 1));

        /// <summary>
        /// Create new ToDo.
        /// </summary>
        /// <param name="command">Data for creating todo.</param>
        /// <response code="201">Created. ToDo id in body.</response>
        /// <returns>
        /// ToDo id.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> CreateToDo([FromBody] CreateToDoCommand command)
        {
            command.UserId = 1;

            return await this.SendCreateCommand(command, nameof(GetToDo));
        }
    }
}
