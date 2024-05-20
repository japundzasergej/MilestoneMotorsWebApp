using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetSingleCarQueryHandler(ICarsRepository carsRepository, IMapper mapper)
        : IRequestHandler<GetSingleCarQuery, CarDto>
    {
        public async Task<CarDto> Handle(
            GetSingleCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var carDetail =
                await carsRepository.GetCarByIdAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");

            return mapper.Map<CarDto>(carDetail);
        }
    }
}
