using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class DeleteUserCommandHandler(
        IUserRepository userRepository,
        SignInManager<User> signInManager
    ) : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await userRepository.GetByIdAsync(request.Id)
                ?? throw new InvalidDataException("Object doesn't exist");

            await userRepository.Delete(user);
            await signInManager.SignOutAsync();
            return true;
        }
    }
}
