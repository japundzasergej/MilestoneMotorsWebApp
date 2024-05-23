using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class EditCarQueryHandler(ICarsRepository carsRepository, IMapper mapper)
        : IRequestHandler<EditCarQuery, EditCarDto>
    {
        public async Task<EditCarDto> Handle(
            EditCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var userCar =
                await carsRepository.GetCarByIdAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");
            return mapper.Map<EditCarDto>(userCar);
        }
    }
}
