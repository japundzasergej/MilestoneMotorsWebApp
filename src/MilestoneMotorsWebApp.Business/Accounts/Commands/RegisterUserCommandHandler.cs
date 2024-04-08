using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Accounts.Commands;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Helpers;
using MilestoneMotorsWebApp.Domain.Constants;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Accounts.Commands;

public class RegisterUserCommandHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterUserCommand, ResponseDTO>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<ResponseDTO> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var registerDto = request.RegisterUserDto;
        var registerFeedbackDto = new RegisterUserFeedbackDto();
        try
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);

            if (user != null)
            {
                registerFeedbackDto.UserExists = true;
                return PopulateResponseDto.OnSuccess(registerFeedbackDto, 200);
            }
            var newUser = new User
            {
                Email = registerDto.Email.Trim(),
                UserName = registerDto.Username.Trim(),
                ProfilePictureImageUrl = "",
            };
            var response = await _userManager.CreateAsync(newUser, registerDto.Password);
            if (response.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return PopulateResponseDto.OnSuccess(registerFeedbackDto, 200);
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

                return PopulateResponseDto.OnSuccess(registerFeedbackDto, 200);
            }
        }
        catch (Exception e)
        {
            return PopulateResponseDto.OnError(e);
        }
    }
}
