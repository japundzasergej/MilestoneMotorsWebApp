using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
