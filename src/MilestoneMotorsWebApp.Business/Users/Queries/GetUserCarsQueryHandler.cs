using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserCarsQueryHandler(IUserRepository userRepository, IMapper mapper)
        : IRequestHandler<GetUserCarsQuery, IEnumerable<CarDto>>
    {
        public async Task<IEnumerable<CarDto>> Handle(
            GetUserCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var result =
                await userRepository.GetUserCarsAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");

            return result.Select(mapper.Map<CarDto>);
        }
    }
}
