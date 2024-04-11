using AutoMapper;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class EditUserQueryHandler(IMapper mapper, IUserRepository userRepository)
        : IRequestHandler<EditUserQuery, EditUserDto>
    {
        public async Task<EditUserDto> Handle(
            EditUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var userPage =
                await userRepository.GetByIdAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");

            return mapper.Map<EditUserDto>(userPage);
        }
    }
}
