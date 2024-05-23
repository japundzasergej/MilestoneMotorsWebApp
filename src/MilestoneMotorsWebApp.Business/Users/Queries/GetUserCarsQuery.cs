using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserCarsQuery : IRequest<IEnumerable<CarDto>>
    {
        [FromRoute]
        public string Id { get; init; }
    }
}
