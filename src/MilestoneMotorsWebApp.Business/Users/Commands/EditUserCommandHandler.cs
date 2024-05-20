using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users.Commands
{
    public class EditUserCommandHandler(
        IUserRepository userRepository,
        IPhotoService photoService,
        UserManager<User> userManager
    ) : IRequestHandler<EditUserCommand, EditUserFeedbackDto>
    {
        public async Task<EditUserFeedbackDto> Handle(
            EditUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var editDto = request.EditUserDto;
            var contentType = editDto.ImageContentType ?? "";

            var user =
                await userRepository.GetByIdAsync(editDto.Id)
                ?? throw new InvalidDataException("Object doesn't exist");
            var editFeedbackDto = new EditUserFeedbackDto();

            if (editDto.ProfilePictureImageUrl != null && editDto.ProfilePictureImageUrl.Length > 0)
            {
                var photoResult = (string?)
                    await photoService.CloudinaryUpload(
                        editDto.ProfilePictureImageUrl,
                        contentType
                    );

                if (photoResult == null)
                {
                    editFeedbackDto.IsImageServiceDown = true;
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureImageUrl))
                {
                    _ = photoService.DeletePhotoAsync(user.ProfilePictureImageUrl);
                }

                user.ProfilePictureImageUrl = photoResult ?? user.ProfilePictureImageUrl;
                user.City = editDto?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editDto?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editDto?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return editFeedbackDto;
                }
                else
                {
                    throw new BadHttpRequestException("Invalid request format.");
                }
            }
            else
            {
                user.City = editDto?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editDto?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editDto?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return editFeedbackDto;
                }
                else
                {
                    throw new BadHttpRequestException("Invalid request format.");
                }
            }
        }
    }
}
