using System.Text;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(IUserService userService)
        : BaseController<IUserService>(userService)
    {
        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> Detail(string? id)
        {
            var result = await _service.GetUserDetail(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> EditPage(string? id)
        {
            var result = await _service.GetUserEdit(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> EditPage(EditUserViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();
                var editUserFeedback = await _service.PostUserEdit(userId, editVM);

                if (editUserFeedback == null)
                {
                    TempData["Error"] = "Something went wrong, please try again.";
                    return RedirectToAction("Detail", "User", new { id = userId });
                }

                if (editUserFeedback.IsImageServiceDown)
                {
                    TempData["Error"] = "Image upload service is down.";
                }

                if (!editUserFeedback.IsAuthorized)
                {
                    return Unauthorized();
                }

                if (!editUserFeedback.HasFailed)
                {
                    TempData["Success"] = "Successfully updated profile.";
                    return RedirectToAction("Detail", "User", new { id = userId });
                }
                else
                {
                    TempData["Error"] = "Something went wrong, please try again.";
                    return RedirectToAction("Detail", "User", new { id = userId });
                }
            }
            return View(editVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> MyListings()
        {
            var userId = GetUserId();
            var result = await _service.GetUserCars(userId);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = GetUserId();
            var response = await _service.DeleteUser(userId);

            if (response == null)
            {
                return NotFound();
            }

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("JwtToken");
                TempData["Success"] = "Account successfully deleted.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] =
                    "Something went wrong while deleting your account, please try again later.";
                return RedirectToAction("Detail", "User", new { userId });
            }
        }
    }
}
