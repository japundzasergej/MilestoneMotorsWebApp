using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserDetailQueryHandler(IUserRepository userRepository, IMapper mapper)
        : IRequestHandler<GetUserDetailQuery, UserDto>
    {
        public async Task<UserDto> Handle(
            GetUserDetailQuery request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                throw new InvalidDataException("Object doesn't exist");
            }

            var userPage =
                await userRepository.GetByIdAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");

            return mapper.Map<UserDto>(userPage);
        }
    }
}
