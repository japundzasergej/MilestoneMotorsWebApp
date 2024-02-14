using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class EditUserQuery : IRequest<EditUserDto?>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
