using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Business.Cars.Queries;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController(IMediator mediator) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCars(
            [FromQuery] string? search,
            [FromQuery] string? orderBy,
            [FromQuery] string? fuelType,
            [FromQuery] string? condition,
            [FromQuery] string? brand
        )
        {
            var response = await mediator.Send(
                new GetAllCarsQuery
                {
                    Search = search,
                    OrderBy = orderBy,
                    FuelType = fuelType,
                    Condition = condition,
                    Brand = brand
                }
            );
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetSingleCar([FromRoute] int id)
        {
            var response = await mediator.Send(new GetSingleCarQuery { Id = id });
            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDto dto)
        {
            var response = await mediator.Send(new CreateCarCommand { CreateCarDto = dto });
            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("edit/{id:int}")]
        public async Task<IActionResult> GetEditCar([FromRoute] int id)
        {
            var response = await mediator.Send(new EditCarQuery { Id = id });
            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> PostEditCar([FromBody] EditCarDto dto)
        {
            var response = await mediator.Send(new EditCarCommand { EditCarDto = dto });
            return Ok(response);
        }

        [Authorize]
        [HttpDelete]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteCar([FromRoute] int id)
        {
            var response = await mediator.Send(new DeleteCarCommand { Id = id });
            return Ok(response);
        }
    }
}
