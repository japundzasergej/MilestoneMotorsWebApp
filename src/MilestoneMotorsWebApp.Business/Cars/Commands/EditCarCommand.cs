using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class EditCarCommand : IRequest<ResponseDTO>
    {
        [FromBody]
        public EditCarDto EditCarDto { get; set; }
    }
}
