using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDTO> GetUserDetail([FromRoute] GetUserDetailQuery query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<ResponseDTO> GetEditUser([FromRoute] EditUserQuery query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<ResponseDTO> PostEditUser([FromBody] EditUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpGet]
        [Route("userCars/{id}")]
        public async Task<ResponseDTO> GetUserCars([FromRoute] GetUserCarsQuery query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ResponseDTO> DeleteUser([FromRoute] DeleteUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        [Route("profilePicture/{id}")]
        public async Task<ResponseDTO> GetProfilePicture([FromRoute] GetProfilePictureQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
