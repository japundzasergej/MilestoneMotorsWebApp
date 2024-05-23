using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetProfilePictureQuery : IRequest<string>
    {
        [FromRoute]
        public string Id { get; init; }
    }
}
