using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Users.Commands;
using MilestoneMotorsWebApp.Business.Users.Queries;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserDetail([FromRoute] string id)
        {
            var response = await mediator.Send(new GetUserDetailQuery { Id = id });
            return Ok(response);
        }

        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> GetEditUser([FromRoute] string id)
        {
            var response = await mediator.Send(new EditUserQuery { Id = id });
            return Ok(response);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PostEditUser([FromBody] EditUserDto dto)
        {
            var response = await mediator.Send(new EditUserCommand { EditUserDto = dto });
            return Ok(response);
        }

        [HttpGet]
        [Route("userCars/{id}")]
        public async Task<IActionResult> GetUserCars([FromRoute] string id)
        {
            var response = await mediator.Send(new GetUserCarsQuery { Id = id });
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var response = await mediator.Send(new DeleteUserCommand { Id = id });
            return Ok(response);
        }

        [HttpGet]
        [Route("profilePicture/{id}")]
        public async Task<IActionResult> GetProfilePicture([FromRoute] string id)
        {
            var response = await mediator.Send(new GetProfilePictureQuery { Id = id });
            return Ok(response);
        }
    }
}
