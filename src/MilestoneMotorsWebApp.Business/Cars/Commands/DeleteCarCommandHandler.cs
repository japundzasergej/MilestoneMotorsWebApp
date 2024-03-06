using MediatR;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class DeleteCarCommandHandler(ICarsRepository carsRepository)
        : IRequestHandler<DeleteCarCommand, bool>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;

        public async Task<bool> Handle(
            DeleteCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            var userCar = await _carsRepository.GetCarByIdAsync(id);
            return await _carsRepository.Remove(userCar);
        }
    }
}
