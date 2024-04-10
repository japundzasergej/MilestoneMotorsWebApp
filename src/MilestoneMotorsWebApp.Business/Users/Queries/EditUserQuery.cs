using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class EditUserQuery : IRequest<ResponseDTO>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
