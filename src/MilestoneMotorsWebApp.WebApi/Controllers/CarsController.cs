using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Business.Cars.Queries;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController(IMediator mediator) : BaseController(mediator)
    {
        [HttpGet]
        public async Task<List<Car>> GetAllCars([FromQuery] GetAllCarsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<Car?> GetSingleCar([FromRoute] GetSingleCarQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ImageServiceDto> CreateCar([FromBody] CreateCarCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        [Route("edit/{id:int}")]
        public async Task<EditCarDto> GetEditCar([FromRoute] EditCarQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<bool?> PostEditCar([FromBody] EditCarCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<bool> DeleteCar([FromRoute] DeleteCarCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
