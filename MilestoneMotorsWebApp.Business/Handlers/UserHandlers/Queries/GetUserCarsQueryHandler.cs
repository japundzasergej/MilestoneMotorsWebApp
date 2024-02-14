using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilestoneMotorsWebApp.Business.Users.Queries;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;
using MilestoneMotorsWebApp.Infrastructure.Repositories;

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
