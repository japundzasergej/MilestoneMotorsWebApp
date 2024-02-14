using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
