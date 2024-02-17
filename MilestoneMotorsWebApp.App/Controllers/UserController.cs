using System.Text;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Common.ViewModels;
using MilestoneMotorsWebApp.Domain.Entities;
using Newtonsoft.Json;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(
        IMapperService mapperService,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration
    ) : BaseController(mapperService, httpClientFactory, configuration)
    {
        public override UriBuilder CloneApiUrl()
        {
            return base.CloneApiUrl().ExtendPath("user");
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> Detail(string? id)
        {
            var apiUrl = CloneApiUrl().ExtendPath($"/{id}").ToString();
            using var client = GetClientFactory();
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

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> EditPage(string? id)
        {
            var apiUrl = CloneApiUrl().ExtendPath($"/edit/{id}").ToString();

            using var client = GetClientFactory();
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

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> EditPage(EditUserViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editVM);
            }

            var apiUrl = CloneApiUrl().ExtendPath("/edit").ToString();
            using var client = GetClientFactory();

            var userId = GetUserId();

            var editDto = _mapperService.Map<EditUserViewModel, EditUserDto>(editVM);
            editDto.Id = userId;
            editDto.ImageContentType = editVM?.ProfilePictureImageUrl?.ContentType ?? "";

            var payload = new { EditUserDto = editDto };

            var jsonEditDto = new StringContent(
                JsonConvert.SerializeObject(payload),
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
                    return RedirectToAction("Detail", "User", new { editDto.Id });
                }
                else
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Detail", "User", new { editDto.Id });
                }
            }
            else
            {
                TempData["Error"] = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Detail", "User", new { editDto.Id });
            }
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        public async Task<IActionResult> MyListings()
        {
            var userId = GetUserId();
            var apiUrl = CloneApiUrl().ExtendPath($"/userCars/{userId}").ToString();
            using var client = GetClientFactory();

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

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpPost]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = GetUserId();
            var apiUrl = CloneApiUrl().ExtendPath($"/delete/{userId}").ToString();
            using var client = GetClientFactory();

            var response = await client.PostAsync(apiUrl, new StringContent(string.Empty));

            if (userId == null)
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
