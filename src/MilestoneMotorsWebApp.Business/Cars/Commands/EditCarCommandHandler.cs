using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class EditCarCommandHandler(ICarsRepository carsRepository, IMapperService mapperService)
        : IRequestHandler<EditCarCommand, ResponseDTO>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            EditCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var carDto = request.EditCarDto;
            var id = carDto.Id;
            if (id == 0)
            {
                return PopulateResponseDto.OnFailure(404);
            }
            var userCar = await _carsRepository.GetCarByIdNoTrackAsync(id);

            if (userCar == null)
            {
                return PopulateResponseDto.OnFailure(404);
            }

            try
            {
                var car = _mapperService.Map<EditCarDto, Car>(carDto);

                car.Id = id;
                car.UserId = userCar.UserId;
                car.HeadlinerImageUrl = userCar.HeadlinerImageUrl;
                car.ImagesUrl = userCar.ImagesUrl;
                car.AdNumber = userCar.AdNumber;
                car.CreatedAt = userCar.CreatedAt;

                var result = await _carsRepository.Update(car);

                if (!result)
                {
                    return PopulateResponseDto.OnFailure(400);
                }

                return PopulateResponseDto.OnSuccess(result, 200);
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
