using MediatR;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserCarsQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetUserCarsQuery, IEnumerable<Car>?>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<Car>?> Handle(
            GetUserCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return await _userRepository.GetUserCarsAsync(id);
        }
    }
}
