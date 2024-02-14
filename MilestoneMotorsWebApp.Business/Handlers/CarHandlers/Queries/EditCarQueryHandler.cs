using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilestoneMotorsWebApp.Business.Cars.Queries;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.CarHandlers.Queries
{
    public class EditCarQueryHandler(ICarsRepository carsRepository)
        : BaseHandler<EditCarQuery, EditCarDto?, ICarsRepository>(carsRepository, null)
    {
        public override async Task<EditCarDto?> Handle(
            EditCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            if (id == 0)
            {
                return null;
            }

            var userCar = await _repository.GetCarByIdAsync(id);

            if (userCar == null)
            {
                return null;
            }
            EditCarDto carDto = _mapperService.Map<Car, EditCarDto>(userCar);
            var capacity = userCar.EngineCapacity.Split(" ");
            var mileage = userCar.Mileage.Split(" ");
            var enginePower = userCar.EnginePower.Split(" ");
            char[] invalidChars =  [ ' ', '€' ];
            string cleanString =
                new(userCar.Price.Trim().Where(c => !invalidChars.Contains(c)).ToArray());
            if (double.TryParse(cleanString, out double price))
            {
                carDto.Price = price;
            }
            else
            {
                carDto.Price = default;
            }
            if (int.TryParse(capacity[0], out int engineCapacity))
            {
                carDto.EngineCapacity = engineCapacity;
            }
            else
            {
                carDto.EngineCapacity = default;
            }
            carDto.Mileage = mileage[0];
            carDto.EnginePower = enginePower[0];

            return carDto;
        }
    }
}
