﻿using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.CarHandlers.Commands
{
    public class EditCarCommandHandler(ICarsRepository carsRepository, IMapperService mapperService)
        : BaseHandler<EditCarCommand, bool?, ICarsRepository>(carsRepository, mapperService)
    {
        public override async Task<bool?> Handle(
            EditCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var carDto = request.EditCarDto;
            var id = carDto.Id;
            if (id == 0)
            {
                return null;
            }
            var userCar = await _repository.GetCarByIdNoTrackAsync(id);

            if (userCar == null)
            {
                return null;
            }

            var car = _mapperService.Map<EditCarDto, Car>(carDto);

            car.Id = id;
            car.UserId = userCar.UserId;
            car.HeadlinerImageUrl = userCar.HeadlinerImageUrl;
            car.ImagesUrl = userCar.ImagesUrl;
            car.AdNumber = userCar.AdNumber;
            car.CreatedAt = userCar.CreatedAt;

            return await _repository.Update(car);
        }
    }
}
