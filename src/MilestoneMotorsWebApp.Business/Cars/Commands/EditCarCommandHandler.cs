using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class EditCarCommandHandler(ICarsRepository carsRepository, IMapperService mapperService)
        : IRequestHandler<EditCarCommand, bool?>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<bool?> Handle(EditCarCommand request, CancellationToken cancellationToken)
        {
            var carDto = request.EditCarDto;
            var id = carDto.Id;
            if (id == 0)
            {
                return null;
            }
            var userCar = await _carsRepository.GetCarByIdNoTrackAsync(id);

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

            return await _carsRepository.Update(car);
        }
    }
}
