using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class EditCarCommandHandler(ICarsRepository carsRepository, IMapper mapper)
        : IRequestHandler<EditCarCommand, bool>
    {
        public async Task<bool> Handle(EditCarCommand request, CancellationToken cancellationToken)
        {
            var userCar =
                await carsRepository.GetCarByIdNoTrackAsync(request.EditCarDto.Id)
                ?? throw new InvalidDataException("Object doesn't exist.");
            var car = mapper.Map<Car>(request.EditCarDto);

            car.HeadlinerImageUrl = userCar.HeadlinerImageUrl;
            car.ImagesUrl = userCar.ImagesUrl;
            car.AdNumber = userCar.AdNumber;
            car.CreatedAt = userCar.CreatedAt;

            return await carsRepository.Update(car);
        }
    }
}
