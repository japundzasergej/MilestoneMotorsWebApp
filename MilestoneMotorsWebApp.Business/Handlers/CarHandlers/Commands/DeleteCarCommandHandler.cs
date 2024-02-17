using MilestoneMotorsWebApp.Business.Cars.Commands;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.CarHandlers.Commands
{
    public class DeleteCarCommandHandler(ICarsRepository carsRepository)
        : BaseHandler<DeleteCarCommand, bool, ICarsRepository>(carsRepository, null)
    {
        public override async Task<bool> Handle(
            DeleteCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            var userCar = await _repository.GetCarByIdAsync(id);
            return await _repository.Remove(userCar);
        }
    }
}
