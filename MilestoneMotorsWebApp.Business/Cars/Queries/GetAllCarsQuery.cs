using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetAllCarsQuery : IRequest<List<Car>>
    {
        [FromQuery]
        public string? Search { get; set; }

        [FromQuery]
        public string? OrderBy { get; set; }

        [FromQuery]
        public string? FuelType { get; set; }

        [FromQuery]
        public string? Condition { get; set; }

        [FromQuery]
        public string? Brand { get; set; }
    }
}
