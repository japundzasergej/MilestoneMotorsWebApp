using MediatR;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class DeleteCarCommandHandler(ICarsRepository carsRepository)
        : IRequestHandler<DeleteCarCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var userCar = await carsRepository.GetCarByIdAsync(request.Id);

            return userCar is null
                ? throw new InvalidDataException("Object doesn't exist.")
                : await carsRepository.Remove(userCar);
        }
    }
}
