using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class LoginUserCommand : IRequest<ResponseDTO>
    {
        [FromBody]
        public LoginUserDto LoginUserDto { get; set; }
    }
}
