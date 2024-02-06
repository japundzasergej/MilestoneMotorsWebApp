using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(IUserCommand userCommand, UserManager<User> userManager)
        : Controller
    {
        private readonly IUserCommand _userCommand = userCommand;
        private readonly UserManager<User> _userManager = userManager;

        [Authorize]
        public async Task<IActionResult> Detail(string? id)
        {
            var userPage = await _userCommand.GetUserDetail(id);
            if (userPage == null)
            {
                return NotFound();
            }
            return View(userPage);
        }

        [Authorize]
        public async Task<IActionResult> EditPage(string? id)
        {
            var userVM = await _userCommand.GetEditUser(id);
            if (userVM == null)
            {
                return NotFound();
            }
            return View(userVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPage(EditUserViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editVM);
            }

            object onImageServiceDown() => TempData["Error"] = "Image upload service is down.";
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var newUser = await _userCommand.PostEditUser(editVM, user, onImageServiceDown);

            if (newUser)
            {
                TempData["Success"] = "Successfully updated profile.";
                return RedirectToAction("Detail", "User", new { user.Id });
            }
            else
            {
                TempData["Error"] = "Something went wrong.";
                return RedirectToAction("Detail", "User", new { user.Id });
            }
        }

        [Authorize]
        public async Task<IActionResult> MyListings()
        {
            var currentUser = HttpContext.User.GetUserId();
            var userCars = await _userCommand.GetUserCars(currentUser);
            return View(userCars);
        }

        [HttpPost]
        [Authorize]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }
            var result = await _userCommand.DeleteUserProfile(currentUser);
            if (result)
            {
                TempData["Success"] = "Account successfully deleted.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] =
                    "Something went wrong while deleting your account, please try again later.";
                return RedirectToAction("Detail", "User", new { currentUser.Id });
            }
        }
    }
}
