using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Common.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(IMapperService mapperService, IHttpClientFactory httpClientFactory)
        : BaseController(mapperService, httpClientFactory)
    {
        public override UriBuilder CloneApiUrl()
        {
            return base.CloneApiUrl().ExtendPath("user");
        }

        [Authorize]
        public async Task<IActionResult> Detail(string? id)
        {
            var apiUrl = CloneApiUrl().ExtendPath($"/{id}").ToString();
            using var client = _httpClientFactory.CreateClient();
            var result = await client.GetAsync(apiUrl);

            if (result.IsSuccessStatusCode)
            {
                string responseBody = await result.Content.ReadAsStringAsync();
                var userPage = JsonConvert.DeserializeObject<User>(responseBody);

                if (userPage == null)
                {
                    return NotFound();
                }

                return View(userPage);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        public async Task<IActionResult> EditPage(string? id)
        {
            var apiUrl = CloneApiUrl().ExtendPath($"/edit/{id}").ToString();

            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var userDto = JsonConvert.DeserializeObject<EditUserDto>(responseBody);

                if (userDto == null)
                {
                    return NotFound();
                }

                var userVM = _mapperService.Map<EditUserDto, EditUserViewModel>(userDto);

                return View(userVM);
            }
            else
            {
                return NotFound();
            }
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

            var apiUrl = CloneApiUrl().ExtendPath("/edit").ToString();
            using var client = _httpClientFactory.CreateClient();

            var userId = HttpContext.User.GetUserId();

            var editDto = _mapperService.Map<EditUserViewModel, EditUserDto>(editVM);
            editDto.Id = userId;

            var jsonEditDto = new StringContent(
                JsonConvert.SerializeObject(editDto),
                Encoding.UTF8,
                "application/json"
            );
            var response = await client.PostAsync(apiUrl, jsonEditDto);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var editUserFeedback = JsonConvert.DeserializeObject<EditUserFeedbackDto>(
                    responseBody
                );
                if (editUserFeedback.IsImageServiceDown)
                {
                    TempData["Error"] = "Image upload service is down.";
                }
                if (!editUserFeedback.HasFailed)
                {
                    TempData["Success"] = "Successfully updated profile.";
                    return RedirectToAction("Detail", "User", new { userId });
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return RedirectToAction("Detail", "User", new { userId });
                }
            }
            else
            {
                TempData["Error"] = "Something went wrong";
                return RedirectToAction("Detail", "User", new { userId });
            }
        }

        [Authorize]
        public async Task<IActionResult> MyListings()
        {
            var id = HttpContext.User.GetUserId();
            var apiUrl = CloneApiUrl().ExtendPath($"/userCars/{id}").ToString();
            using var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonUserCars = JsonConvert.DeserializeObject<List<Car>>(responseBody);
                return View(jsonUserCars);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var id = HttpContext.User.GetUserId();
            var apiUrl = CloneApiUrl().ExtendPath($"/delete/{id}").ToString();
            using var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(apiUrl, new StringContent(string.Empty));

            if (id == null)
            {
                return NotFound();
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Account successfully deleted.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] =
                    "Something went wrong while deleting your account, please try again later.";
                return RedirectToAction("Detail", "User", new { id });
            }
        }
    }
}
