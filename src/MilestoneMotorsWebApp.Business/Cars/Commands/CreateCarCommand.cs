using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class CreateCarCommand : IRequest<ImageServiceDto>
    {
        [FromBody]
        public CreateCarDto CreateCarDto { get; set; }
    }
}
