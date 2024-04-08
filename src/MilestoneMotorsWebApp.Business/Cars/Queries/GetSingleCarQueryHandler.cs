using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetSingleCarQueryHandler(
        ICarsRepository carsRepository,
        IMapperService mapperService
    ) : IRequestHandler<GetSingleCarQuery, ResponseDTO>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            GetSingleCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            if (id == 0)
            {
                return PopulateResponseDto.OnFailure(404);
            }

            try
            {
                var carDetail = await _carsRepository.GetCarByIdAsync(id);

                if (carDetail == null)
                {
                    return PopulateResponseDto.OnFailure(404);
                }
                return PopulateResponseDto.OnSuccess(
                    _mapperService.Map<Car, CarDto>(carDetail),
                    200
                );
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
