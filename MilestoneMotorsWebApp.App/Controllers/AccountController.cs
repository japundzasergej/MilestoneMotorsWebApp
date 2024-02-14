using System.Text;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Common.ViewModels;
using Newtonsoft.Json;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(IMapperService mapperService, IHttpClientFactory clientFactory)
        : BaseController(mapperService, clientFactory)
    {
        public override UriBuilder CloneApiUrl()
        {
            return base.CloneApiUrl().ExtendPath("account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var registerVM = new RegisterUserViewModel();
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var userDto = _mapperService.Map<RegisterUserViewModel, RegisterUserDto>(
                    registerVM
                );
                string apiUrl = CloneApiUrl().ExtendPath("/register").ToString();
                using var client = _httpClientFactory.CreateClient();
                var jsonUserDto = new StringContent(
                    JsonConvert.SerializeObject(userDto),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(apiUrl, jsonUserDto);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var registerFeedbackDto =
                        JsonConvert.DeserializeObject<RegisterUserFeedbackDto>(responseBody);

                    if (registerFeedbackDto.UserExists)
                    {
                        TempData["Error"] = "User already exists with that username.";
                        return View(registerVM);
                    }

                    if (registerFeedbackDto.ResponseFailed)
                    {
                        foreach (var error in registerFeedbackDto.ErrorList)
                        {
                            TempData["Error"] = error;
                        }
                    }

                    TempData["Success"] = "Account successfully created!";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return View(registerVM);
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginVM = new LoginUserViewModel();
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var loginDto = _mapperService.Map<LoginUserViewModel, LoginUserDto>(loginVM);
                var apiUrl = CloneApiUrl().ExtendPath("/login").ToString();

                var jsonLoginDto = new StringContent(
                    JsonConvert.SerializeObject(loginDto),
                    Encoding.UTF8,
                    "application/json"
                );

                using var client = _httpClientFactory.CreateClient();

                var response = await client.PostAsync(apiUrl, jsonLoginDto);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var loginFeedbackDto = JsonConvert.DeserializeObject<LoginUserFeedbackDto>(
                        responseBody
                    );

                    if (!loginFeedbackDto.IsValidUser)
                    {
                        TempData["Error"] = "No user registered with that email.";
                        return View(loginVM);
                    }

                    if (loginFeedbackDto.IsNotPasswordsMatching)
                    {
                        TempData["Error"] = "Invalid password.";
                        return View(loginVM);
                    }

                    TempData["Success"] = "Login successful!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return View(loginVM);
                }
            }
            return View(loginVM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var apiUrl = CloneApiUrl().ExtendPath("/logout").ToString();
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
