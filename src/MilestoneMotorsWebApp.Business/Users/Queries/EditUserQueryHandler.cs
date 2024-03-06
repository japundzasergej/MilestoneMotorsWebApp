using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class EditUserQueryHandler(IMapperService mapperService, IUserRepository userRepository)
        : IRequestHandler<EditUserQuery, EditUserDto?>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<EditUserDto?> Handle(
            EditUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var userPage = await _userRepository.GetByIdAsync(id);

            if (userPage == null)
            {
                return null;
            }

            var userDto = _mapperService.Map<User, EditUserDto>(userPage);
            return userDto;
        }
    }
}
