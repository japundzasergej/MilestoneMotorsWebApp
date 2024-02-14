using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.DTO;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands
{
    public class RegisterUserCommand : IRequest<RegisterUserFeedbackDto>
    {
        [FromBody]
        public RegisterUserDto RegisterUserDto { get; set; }
    }
}
