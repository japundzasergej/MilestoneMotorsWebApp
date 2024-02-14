using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetSingleCarQuery : IRequest<Car?>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
