using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.DTO;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class LoginUserCommand : IRequest<LoginUserFeedbackDto>
    {
        [FromBody]
        public LoginUserDto LoginUserDto { get; set; }
    }
}
