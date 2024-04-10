using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Cars.Commands
{
    public class DeleteCarCommandHandler(ICarsRepository carsRepository)
        : IRequestHandler<DeleteCarCommand, ResponseDTO>
    {
        private readonly ICarsRepository _carsRepository = carsRepository;

        public async Task<ResponseDTO> Handle(
            DeleteCarCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            try
            {
                var userCar = await _carsRepository.GetCarByIdAsync(id);
                var result = await _carsRepository.Remove(userCar);
                return PopulateResponseDto.OnSuccess(result, 202);
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
