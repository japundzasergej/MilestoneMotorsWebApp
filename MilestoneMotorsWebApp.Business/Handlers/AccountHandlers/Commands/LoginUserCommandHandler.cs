using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWeb.Business.Handlers.AccountHandlers.Commands
{
    public class LoginUserCommandHandler(
        SignInManager<User> signInManager,
        UserManager<User> userManager
    ) : IRequestHandler<LoginUserCommand, LoginUserFeedbackDto>
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<LoginUserFeedbackDto> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var loginDto = request.LoginUserDto;
            var loginFeedbackDto = new LoginUserFeedbackDto();
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        loginDto.Password,
                        false,
                        false
                    );
                    if (result.Succeeded)
                    {
                        return loginFeedbackDto;
                    }
                }
                loginFeedbackDto.IsNotPasswordsMatching = true;
                return loginFeedbackDto;
            }
            loginFeedbackDto.IsValidUser = false;
            return loginFeedbackDto;
        }
    }
}
