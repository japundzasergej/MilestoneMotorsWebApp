using MediatR;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserDetailQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUserDetailQuery, User?>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User?> Handle(
            GetUserDetailQuery request,
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
            return userPage;
        }
    }
}
