using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetAllCarsQuery : IRequest<IEnumerable<CarDto>>
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
