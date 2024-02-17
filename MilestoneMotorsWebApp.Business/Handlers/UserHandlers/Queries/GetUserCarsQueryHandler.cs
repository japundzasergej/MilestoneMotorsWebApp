using MilestoneMotorsWebApp.Business.Users.Queries;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.UserHandlers.Queries
{
    public class GetUserCarsQueryHandler(IUserRepository usersRepository)
        : BaseHandler<GetUserCarsQuery, IEnumerable<Car?>, IUserRepository>(usersRepository, null)
    {
        public override async Task<IEnumerable<Car?>> Handle(
            GetUserCarsQuery request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return await _repository.GetUserCarsAsync(id);
        }
    }
}
