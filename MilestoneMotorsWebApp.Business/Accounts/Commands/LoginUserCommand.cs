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
    public class LoginUserCommand : IRequest<LoginUserFeedbackDto>
    {
        [FromBody]
        public LoginUserDto LoginUserDto { get; set; }
    }
}
