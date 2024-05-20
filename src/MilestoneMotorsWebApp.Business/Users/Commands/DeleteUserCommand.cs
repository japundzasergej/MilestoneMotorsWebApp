using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        [FromRoute]
        public string Id { get; init; }
    }
}
