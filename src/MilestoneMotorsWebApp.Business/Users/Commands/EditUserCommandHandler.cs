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
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<EditUserFeedbackDto> Handle(
            EditUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var editDto = request.EditUserDto;
            var id = editDto.Id;
            var contentType = editDto.ImageContentType ?? "";
            var user = await _userRepository.GetByIdAsync(id);
            var editFeedbackDto = new EditUserFeedbackDto();

            if (editDto.ProfilePictureImageUrl != null && editDto.ProfilePictureImageUrl.Length > 0)
            {
                var photoResult = (string?)
                    await _photoService.CloudinaryUpload(
                        editDto.ProfilePictureImageUrl,
                        contentType
                    );

                if (photoResult == null)
                {
                    editFeedbackDto.IsImageServiceDown = true;
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfilePictureImageUrl);
                }

                user.ProfilePictureImageUrl = photoResult ?? user.ProfilePictureImageUrl;
                user.City = editDto?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editDto?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editDto?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return editFeedbackDto;
                }
                else
                {
                    editFeedbackDto.HasFailed = true;
                    return editFeedbackDto;
                }
            }
            else
            {
                user.City = editDto?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editDto?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editDto?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return editFeedbackDto;
                }
                else
                {
                    editFeedbackDto.HasFailed = true;
                    return editFeedbackDto;
                }
            }
        }
    }
}
