using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetProfilePictureQueryHandler(IUserRepository userRepository)
        : IRequestHandler<GetProfilePictureQuery, ResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<ResponseDTO> Handle(
            GetProfilePictureQuery request,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                return PopulateResponseDto.OnFailure(404);
            }
            try
            {
                var result = await _userRepository.GetUserProfilePictureAsync(request.Id);
                if (result == null)
                {
                    return PopulateResponseDto.OnFailure(404);
                }
                return PopulateResponseDto.OnSuccess(result, 200);
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
