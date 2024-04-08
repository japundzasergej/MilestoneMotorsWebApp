using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class DeleteUserCommandHandler(
        IUserRepository userRepository,
        SignInManager<User> signInManager
    ) : IRequestHandler<DeleteUserCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task<ResponseDTO> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return PopulateResponseDto.OnFailure(404);
                }
                await _userRepository.Delete(user);
                await _signInManager.SignOutAsync();
                return PopulateResponseDto.OnSuccess(true, 202);
            }
            catch (Exception e)
            {
                return PopulateResponseDto.OnError(e);
            }
        }
    }
}
