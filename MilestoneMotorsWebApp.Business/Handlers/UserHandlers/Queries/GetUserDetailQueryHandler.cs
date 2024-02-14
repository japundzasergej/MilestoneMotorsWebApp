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
    public class GetUserDetailQueryHandler(IUserRepository userRepository)
        : BaseHandler<GetUserDetailQuery, User?, IUserRepository>(userRepository, null)
    {
        public override async Task<User?> Handle(
            GetUserDetailQuery request,
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
            return userPage;
        }
    }
}
