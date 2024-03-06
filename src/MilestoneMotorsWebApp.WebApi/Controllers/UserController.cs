using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Users.Commands;
using MilestoneMotorsWebApp.Business.Users.Queries;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<User?> GetUserDetail([FromRoute] GetUserDetailQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<EditUserDto?> GetEditUser([FromRoute] EditUserQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<EditUserFeedbackDto> PostEditUser([FromBody] EditUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        [Route("userCars/{id}")]
        public async Task<IEnumerable<Car>> GetUserCars([FromRoute] GetUserCarsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<bool?> DeleteUser([FromRoute] DeleteUserCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
