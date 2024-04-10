using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ResponseDTO> GetAllCars([FromQuery] GetAllCarsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDTO> GetSingleCar([FromRoute] GetSingleCarQuery query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<ResponseDTO> CreateCar([FromBody] CreateCarCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpGet]
        [Route("edit/{id:int}")]
        public async Task<ResponseDTO> GetEditCar([FromRoute] EditCarQuery query)
        {
            return await _mediator.Send(query);
        }

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<ResponseDTO> PostEditCar([FromBody] EditCarCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<ResponseDTO> DeleteCar([FromRoute] DeleteCarCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
