using Kros.ToDos.Api.Application.Queries;
using Kros.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kros.ToDos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="mediator">Mediator.</param>
        public ToDosController(IMediator mediator)
        {
            _mediator = Check.NotNull(mediator, nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>))]
        public async Task<IEnumerable<GetAllToDoHeadersQuery.ToDoHeader>> Get()
            => await _mediator.Send(new GetAllToDoHeadersQuery(1));
    }
}
