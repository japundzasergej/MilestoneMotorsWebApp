using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class EditCarQuery : IRequest<EditCarDto>
    {
        [FromRoute]
        public int Id { get; set; }
    }
}
