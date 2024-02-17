using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Users.Commands;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Handlers.UserHandlers.Commands
{
    public class EditUserCommandHandler(
        IUserRepository userRepository,
        IMapperService mapperService,
        IPhotoService photoService,
        UserManager<User> userManager
    )
        : BaseHandler<EditUserCommand, EditUserFeedbackDto, IUserRepository>(
            userRepository,
            mapperService
        )
    {
        private readonly IPhotoService _photoService = photoService;
        private readonly UserManager<User> _userManager = userManager;

        private async Task<string?> CloudinaryUpload(byte[]? byteArray, string contentType)
        {
            if (byteArray != null || byteArray.Length > 0)
            {
                using var memoryStream = new MemoryStream(byteArray);
                await memoryStream.WriteAsync(byteArray);

                var convertedFile = new FormFile(
                    memoryStream,
                    0,
                    memoryStream.Length,
                    "file",
                    "profilePicture"
                )
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };

                var result = await _photoService.AddPhotoAsync(convertedFile);
                return result?.Url.ToString() ?? "";
            }
            else
            {
                return null;
            }
        }

        public override async Task<EditUserFeedbackDto> Handle(
            EditUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var editDto = request.EditUserDto;
            var id = editDto.Id;
            var contentType = editDto.ImageContentType ?? "";
            var user = await _repository.GetByIdAsync(id);
            var editFeedbackDto = new EditUserFeedbackDto();

            if (editDto.ProfilePictureImageUrl != null && editDto.ProfilePictureImageUrl.Length > 0)
            {
                var photoResult = await CloudinaryUpload(
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
