using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class EditUserCommand : IRequest<EditUserFeedbackDto>
    {
        [FromBody]
        public EditUserDto EditUserDto { get; set; }
    }
}
