using CQRS_Mediator_Dapper_Api.CQRS.Commands.Users.StoredProcedure;
using CQRS_Mediator_Dapper_Api.CQRS.Queries.Users.StoredProcedure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CQRS_Mediator_Dapper_Api.Controllers.StoredProcedure
{
    [Route("api/StoredProcedure/[controller]")]
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
            var result = await _mediator.Send(new GetAllUserSpQuery());
            if (result is null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserByIdSpQuery { Id = id });
            if (result is null)
            {
                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserSpCommand command) => Ok(await _mediator.Send(command));

        [HttpPut]
        public async Task<ActionResult> Update(UpdateUserSpCommand command) => Ok(await _mediator.Send(command));

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) => Ok(await _mediator.Send(new DeleteUserByIdSpCommand { Id = id }));
    }
}
