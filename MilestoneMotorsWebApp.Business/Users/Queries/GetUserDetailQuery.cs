using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserDetailQuery : IRequest<User?>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
