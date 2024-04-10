using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserCarsQueryHandler(
        IUserRepository userRepository,
        IMapperService mapperService
    ) : IRequestHandler<GetUserCarsQuery, ResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            GetUserCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                return PopulateResponseDto.OnFailure(404);
            }

            try
            {
                var result = await _userRepository.GetUserCarsAsync(id);
                if (result == null)
                {
                    return PopulateResponseDto.OnFailure(404);
                }
                return PopulateResponseDto.OnSuccess(
                    result.Select(_mapperService.Map<Car, CarDto>),
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
