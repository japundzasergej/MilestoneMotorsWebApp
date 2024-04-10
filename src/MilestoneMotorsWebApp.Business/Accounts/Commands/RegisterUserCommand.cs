using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class RegisterUserCommand : IRequest<ResponseDTO>
    {
        [FromBody]
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
