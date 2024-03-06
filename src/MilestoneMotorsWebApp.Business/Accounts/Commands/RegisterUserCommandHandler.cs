using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Constants;
using MilestoneMotorsWebApp.Domain.Entities;

public class RegisterUserCommandHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterUserCommand, RegisterUserFeedbackDto>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<RegisterUserFeedbackDto> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var registerDto = request.RegisterUserDto;
        var registerFeedbackDto = new RegisterUserFeedbackDto();
        var user = await _userManager.FindByEmailAsync(registerDto.Email);

        if (user != null)
        {
            registerFeedbackDto.UserExists = true;
            return registerFeedbackDto;
        }
        var newUser = new User
        {
            Email = registerDto.Email.Trim(),
            UserName = registerDto.Username.Trim()
        };
        var response = await _userManager.CreateAsync(newUser, registerDto.Password);
        if (response.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            return registerFeedbackDto;
        }
        else
        {
            registerFeedbackDto.ResponseFailed = true;
            foreach (var error in response.Errors)
            {
                if (error != null)
                {
                    registerFeedbackDto.ErrorList.Add(error);
                }
            }

            return registerFeedbackDto;
        }
    }
}
