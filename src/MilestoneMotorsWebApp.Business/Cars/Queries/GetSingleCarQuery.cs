using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetSingleCarQuery : IRequest<ResponseDTO>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
