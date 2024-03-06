using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class DeleteUserCommandHandler(
        IUserRepository userRepository,
        SignInManager<User> signInManager
    ) : IRequestHandler<DeleteUserCommand, bool?>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task<bool?> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            await _userRepository.Delete(user);
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
