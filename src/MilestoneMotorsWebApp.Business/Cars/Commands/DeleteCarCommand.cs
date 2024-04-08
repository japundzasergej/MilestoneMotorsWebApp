using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class DeleteCarCommand : IRequest<ResponseDTO>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
