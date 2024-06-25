using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetAllCarsQuery : IRequest<IEnumerable<CarDto>>
    {
        [FromQuery]
        public string? Search { get; init; }

        [FromQuery]
        public string? OrderBy { get; init; }

        [FromQuery]
        public string? FuelType { get; init; }

        [FromQuery]
        public string? Condition { get; init; }

        [FromQuery]
        public string? Brand { get; init; }
    }
}
