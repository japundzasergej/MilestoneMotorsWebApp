using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Services;
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

        public override async Task<EditUserFeedbackDto> Handle(
            EditUserCommand request,
            CancellationToken cancellationToken
        )
        {
            var editDto = request.EditUserDto;
            var id = editDto.Id;
            var user = await _repository.GetByIdNoTrackAsync(id);
            var editFeedbackDto = new EditUserFeedbackDto();
            if (editDto.ProfilePictureImageUrl != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editDto.ProfilePictureImageUrl);

                if (photoResult == null)
                {
                    editFeedbackDto.IsImageServiceDown = true;
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfilePictureImageUrl);
                }
                user.ProfilePictureImageUrl =
                    photoResult?.Url.ToString() ?? user.ProfilePictureImageUrl;
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
