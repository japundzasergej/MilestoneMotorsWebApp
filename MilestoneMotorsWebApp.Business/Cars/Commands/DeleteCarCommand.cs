using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class DeleteCarCommand : IRequest<bool>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
