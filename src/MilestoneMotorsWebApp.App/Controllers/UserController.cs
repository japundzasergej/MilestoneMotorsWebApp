using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(IUserService userService, IMapper mapper) : BaseController
    {
        public async Task<IActionResult> Detail(string? id)
        {
            var response = await userService.GetUserDetail(id, GetToken());

            return View(mapper.Map<UserAccountViewModel>(response));
        }

        public async Task<IActionResult> EditPage(string? id)
        {
            var response = await userService.GetUserEdit(id, GetToken());

            return View(mapper.Map<EditUserViewModel>(response));
        }

        [HttpPost]
        public async Task<IActionResult> EditPage(EditUserViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                var editDto = mapper.Map<EditUserDto>(editVM);
                editDto.Id = GetUserId();
                editDto.ImageContentType = editVM.ProfilePictureImageUrl?.ContentType ?? "";
                var response = await userService.PostUserEdit(editDto, GetToken());

                if (response.IsImageServiceDown)
                {
                    TempData["Error"] = "Image service is currently down, please try again later.";
                }
                TempData["Success"] = "Successfully updated profile.";
                return RedirectToAction("Detail", "User", new { id = GetUserId() });
            }
            return View(editVM);
        }

        public async Task<IActionResult> MyListings()
        {
            var response = await userService.GetUserCars(GetUserId(), GetToken());

            return View(new GetUserCarsViewModel { Cars = response });
        }

        [HttpPost]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var response = await userService.DeleteUser(GetUserId(), GetToken());

            if (!response)
            {
                TempData["Error"] =
                    "Something went wrong while deleting your account, please try again later.";
                return RedirectToAction("Detail", "User", new { id = GetUserId() });
            }
            else
            {
                HttpContext.Session.Remove("JwtToken");
                TempData["Success"] = "Account successfully deleted.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
