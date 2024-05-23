using MediatR;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetProfilePictureQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetProfilePictureQuery, string>
    {
        public async Task<string> Handle(
            GetProfilePictureQuery request,
            CancellationToken cancellationToken
        )
        {
            return await userRepository.GetUserProfilePictureAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");
        }
    }
}
