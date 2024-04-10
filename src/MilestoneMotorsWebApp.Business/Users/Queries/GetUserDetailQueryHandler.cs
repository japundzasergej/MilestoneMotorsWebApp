using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserDetailQueryHandler(
        IUserRepository userRepository,
        IMapperService mapperService
    ) : IRequestHandler<GetUserDetailQuery, ResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapperService _mapperService = mapperService;

        public async Task<ResponseDTO> Handle(
            GetUserDetailQuery request,
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
                var userPage = await _userRepository.GetByIdAsync(id);
                if (userPage == null)
                {
                    return PopulateResponseDto.OnFailure(404);
                }
                return PopulateResponseDto.OnSuccess(
                    _mapperService.Map<User, UserDto>(userPage),
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
