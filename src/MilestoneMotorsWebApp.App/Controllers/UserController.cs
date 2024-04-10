using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(IUserService userService, IMvcMapperService mvcMapperService)
        : BaseController<IUserService>(userService, mvcMapperService)
    {
        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> Detail(string? id)
        {
            var response = await _service.GetUserDetail(id, GetToken());
            var error = HandleErrors(response, new());

            if (error != null)
            {
                return error;
            }

            if (response.Body != null)
            {
                return View(
                    _mapperService.Map<UserDto, UserAccountViewModel>(
                        ConvertFromJson<UserDto>(response.Body)
                    )
                );
            }
            TempData["Error"] = "Something went wrong, please try again";
            return RedirectToAction("Index", "Home");
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> EditPage(string? id)
        {
            var response = await _service.GetUserEdit(id, GetToken());

            var error = HandleErrors(response, new());

            if (error != null)
            {
                return error;
            }

            if (response.Body != null)
            {
                return View(
                    _mapperService.Map<EditUserDto, EditUserViewModel>(
                        ConvertFromJson<EditUserDto>(response.Body)
                    )
                );
            }

            TempData["Error"] = "Something went wrong, please try again";
            return RedirectToAction("Index", "Home");
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> EditPage(EditUserViewModel editVM)
        {
            if (ModelState.IsValid)
            {
                var editDto = _mapperService.Map<EditUserViewModel, EditUserDto>(editVM);
                editDto.Id = GetUserId();
                editDto.ImageContentType = editVM.ProfilePictureImageUrl?.ContentType ?? "";
                var response = await _service.PostUserEdit(editDto, GetToken());

                var error = HandleErrors(
                    response,
                    new FailureResponse
                    {
                        StatusCode = 400,
                        ErrorMessage = "Error! Please re-do the form.",
                        ViewModel = editVM
                    }
                );

                if (error != null)
                {
                    return error;
                }

                if (response.Body != null)
                {
                    var editUserFeedbackDto = ConvertFromJson<EditUserFeedbackDto>(response.Body);
                    if (editUserFeedbackDto.IsImageServiceDown)
                    {
                        TempData["Error"] =
                            "Image service is currently down, please try again later.";
                    }
                    TempData["Success"] = "Successfully updated profile.";
                    return RedirectToAction("Detail", "User", new { id = GetUserId() });
                }
            }
            return View(editVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> MyListings()
        {
            var response = await _service.GetUserCars(GetUserId(), GetToken());

            var error = HandleErrors(response, new());

            if (error != null)
            {
                return error;
            }

            if (response.Body != null)
            {
                return View(
                    new GetUserCarsViewModel { Cars = ConvertFromJson<List<CarDto>>(response.Body) }
                );
            }
            TempData["Error"] = "Something went wrong, please try again";
            return RedirectToAction("Index", "Home");
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var response = await _service.DeleteUser(GetUserId(), GetToken());

            var error = HandleErrors(response, new());

            if (error != null)
            {
                return error;
            }

            if (response.Body != null)
            {
                var result = (bool)response.Body;
                if (!result)
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
            TempData["Error"] = "Something went wrong, please try again";
            return RedirectToAction("Index", "Home");
        }
    }
}
