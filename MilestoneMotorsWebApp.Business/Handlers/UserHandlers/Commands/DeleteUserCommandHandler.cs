using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Users.Commands;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.UserHandlers.Commands
{
    public class DeleteUserCommandHandler(
        IUserRepository userRepository,
        SignInManager<User> signInManager
    ) : BaseHandler<DeleteUserCommand, bool, IUserRepository>(userRepository, null)
    {
        private readonly SignInManager<User> _signInManager = signInManager;

        public override async Task<bool> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var id = request.Id;
            var user = await _repository.GetByIdAsync(id);
            await _repository.Delete(user);
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
