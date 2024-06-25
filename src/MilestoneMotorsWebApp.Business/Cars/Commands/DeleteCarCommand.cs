using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class DeleteCarCommand : IRequest<bool>
    {
        [FromRoute]
        public int Id { get; init; }
    }
}
