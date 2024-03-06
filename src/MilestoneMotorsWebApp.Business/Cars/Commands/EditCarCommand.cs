using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class EditCarCommand : IRequest<bool?>
    {
        [FromBody]
        public EditCarDto EditCarDto { get; set; }
    }
}
