using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class EditCarQueryHandler(ICarsRepository carsRepository, IMapperService mapperService)
        : IRequestHandler<EditCarQuery, ResponseDTO>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            EditCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            EditCarDto carDto = new();
            if (id == 0)
            {
                return PopulateResponseDto.OnFailure(404);
            }

            try
            {
                var userCar = await _carsRepository.GetCarByIdAsync(id);

                if (userCar == null)
                {
                    return PopulateResponseDto.OnFailure(404);
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

                return PopulateResponseDto.OnSuccess(carDto, 200);
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
