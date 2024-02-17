using MilestoneMotorsWebApp.Business.Cars.Queries;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.CarHandlers.Queries
{
    public class GetSingleCarQueryHandler(ICarsRepository carsRepository)
        : BaseHandler<GetSingleCarQuery, Car?, ICarsRepository>(carsRepository, null)
    {
        public override async Task<Car?> Handle(
            GetSingleCarQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            if (id == 0)
            {
                return null;
            }

            var carDetail = await _repository.GetCarByIdAsync(id);

            if (carDetail == null)
            {
                return null;
            }
            return carDetail;
        }
    }
}
