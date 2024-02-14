using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Common.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class CreateCarCommand : IRequest<ImageServiceDto>
    {
        [FromBody]
        public CreateCarDto CarDto { get; set; }
    }
}
