using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IMediator mediator) : Controller
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var response = await mediator.Send(new LoginUserCommand { LoginUserDto = dto });
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var response = await mediator.Send(new RegisterUserCommand { RegisterUserDto = dto });
            return Ok(response);
        }
    }
}
