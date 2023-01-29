using CQRS_Mediator_Dapper_Api.CQRS.Commands.Users.Querying;
using CQRS_Mediator_Dapper_Api.CQRS.Queries.Users.Querying;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace CQRS_Mediator_Dapper_Api.Controllers.Querying
{
    [Route("api/Querying/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserQuery());
            if (result is null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { Id = id });
            if (result is null)
            {
                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserCommand command) => Ok(await _mediator.Send(command));

        [HttpPut]
        public async Task<ActionResult> Update(UpdateUserCommand command) => Ok(await _mediator.Send(command));

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) => Ok(await _mediator.Send(new DeleteUserByIdCommand { Id = id}));
    }
}
