using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Utilities;
using MilestoneMotorsWebApp.Infrastructure.Interfaces;

namespace MilestoneMotorsWebApp.Business.Users
{
    public class UserCommands(
        IUserRepository userRepository,
        IPhotoService photoService,
        SignInManager<User> signInManager,
        UserManager<User> userManager
    ) : IUserCommand
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<bool> DeleteUserProfile(User user)
        {
            await _userRepository.Delete(user);
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<EditUserViewModel?> GetEditUser(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var userPage = await _userRepository.GetByIdAsync(id);
            if (userPage == null)
            {
                return null;
            }
            var userViewModel = new EditUserViewModel
            {
                City = userPage.City,
                State = userPage.State,
                Country = userPage.Country,
            };
            return userViewModel;
        }

        public async Task<IEnumerable<Car>?> GetUserCars(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            return await _userRepository.GetUserCarsAsync(id);
        }

        public async Task<User?> GetUserDetail(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }
            var userPage = await _userRepository.GetByIdAsync(id);
            if (userPage == null)
            {
                return null;
            }
            return userPage;
        }

        public async Task<bool> PostEditUser(
            EditUserViewModel editVM,
            User user,
            Func<object> onImageServiceDown
        )
        {
            if (editVM.ProfilePictureImageUrl != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.ProfilePictureImageUrl);

                if (photoResult == null)
                {
                    onImageServiceDown();
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfilePictureImageUrl);
                }

                user.ProfilePictureImageUrl =
                    photoResult?.Url.ToString() ?? user.ProfilePictureImageUrl;
                user.City = editVM?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editVM?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editVM?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                user.City = editVM?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editVM?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editVM?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
