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
