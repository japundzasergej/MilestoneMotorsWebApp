using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetUserDetailQuery : IRequest<UserDto>
    {
        [FromRoute]
        public string Id { get; init; }
    }
}
