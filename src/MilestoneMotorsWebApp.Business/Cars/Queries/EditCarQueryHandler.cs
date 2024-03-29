﻿using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class EditCarQueryHandler(ICarsRepository carsRepository, IMapperService mapperService)
        : IRequestHandler<EditCarQuery, EditCarDto>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<EditCarDto> Handle(
            EditCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            EditCarDto carDto = new();
            if (id == 0)
            {
                carDto.IsSuccessful = false;
                return carDto;
            }

            var userCar = await _carsRepository.GetCarByIdAsync(id);

            if (userCar == null)
            {
                carDto.IsSuccessful = false;
                return carDto;
            }
            carDto = _mapperService.Map<Car, EditCarDto>(userCar);
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
