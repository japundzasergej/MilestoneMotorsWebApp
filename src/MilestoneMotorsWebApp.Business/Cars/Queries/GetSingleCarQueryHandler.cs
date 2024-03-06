using MediatR;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Queries
{
    public class GetSingleCarQueryHandler(ICarsRepository carsRepository)
        : IRequestHandler<GetSingleCarQuery, Car?>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;

        public async Task<Car?> Handle(
            GetSingleCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            if (id == 0)
            {
                return null;
            }

            var carDetail = await _carsRepository.GetCarByIdAsync(id);

            if (carDetail == null)
            {
                return null;
            }
            return carDetail;
        }
    }
}
