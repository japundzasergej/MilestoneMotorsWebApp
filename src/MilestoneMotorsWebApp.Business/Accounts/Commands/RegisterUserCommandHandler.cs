using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Constants;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands;

public class RegisterUserCommandHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterUserCommand, RegisterUserFeedbackDto>
{
    public async Task<RegisterUserFeedbackDto> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var registerDto = request.RegisterUserDto;
        var registerFeedbackDto = new RegisterUserFeedbackDto();

        var user = await userManager.FindByEmailAsync(registerDto.Email);

        if (user != null)
        {
            registerFeedbackDto.UserExists = true;
            return registerFeedbackDto;
        }
        var newUser = new User
        {
            Email = registerDto.Email.Trim(),
            UserName = registerDto.Username.Trim(),
            ProfilePictureImageUrl = "",
        };
        var response = await userManager.CreateAsync(newUser, registerDto.Password);
        if (response.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, UserRoles.User);
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
