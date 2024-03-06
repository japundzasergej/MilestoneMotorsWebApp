using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(IMediator mediator) : BaseController(mediator)
    {
        [HttpPost]
        [Route("login")]
        public async Task<LoginUserFeedbackDto> Login([FromBody] LoginUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost]
        [Route("register")]
        public async Task<RegisterUserFeedbackDto> Register([FromBody] RegisterUserCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
