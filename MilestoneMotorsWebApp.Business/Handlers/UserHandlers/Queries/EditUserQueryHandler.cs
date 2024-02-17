using MilestoneMotorsWebApp.Business.Users.Queries;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.UserHandlers.Queries
{
    public class EditUserQueryHandler(IMapperService mapperService, IUserRepository userRepository)
        : BaseHandler<EditUserQuery, EditUserDto?, IUserRepository>(userRepository, mapperService)
    {
        public override async Task<EditUserDto?> Handle(
            EditUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            var userPage = await _repository.GetByIdAsync(id);

            if (userPage == null)
            {
                return null;
            }

            var userDto = _mapperService.Map<User, EditUserDto>(userPage);
            return userDto;
        }
    }
}
