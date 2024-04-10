using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserCarsQuery : IRequest<ResponseDTO>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
