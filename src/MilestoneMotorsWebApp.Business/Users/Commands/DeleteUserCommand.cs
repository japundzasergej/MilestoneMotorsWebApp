using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class DeleteUserCommand : IRequest<ResponseDTO>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
