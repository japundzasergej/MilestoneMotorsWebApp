using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserCarsQuery : IRequest<IEnumerable<Car?>>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
