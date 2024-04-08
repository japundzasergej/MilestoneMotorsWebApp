using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class EditUserCommand : IRequest<ResponseDTO>
    {
        [FromBody]
        public EditUserDto EditUserDto { get; set; }
    }
}
