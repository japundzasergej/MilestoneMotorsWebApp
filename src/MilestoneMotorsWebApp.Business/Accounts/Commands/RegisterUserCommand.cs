using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class RegisterUserCommand : IRequest<RegisterUserFeedbackDto>
    {
        [FromBody]
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
