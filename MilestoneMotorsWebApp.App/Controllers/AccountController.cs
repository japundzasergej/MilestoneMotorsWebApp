using System.Text;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.Common.DTO;
using MilestoneMotorsWebApp.Common.Interfaces;
using MilestoneMotorsWebApp.Common.Utilities;
using MilestoneMotorsWebApp.Common.ViewModels;
using Newtonsoft.Json;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(
        IMapperService mapperService,
        IHttpClientFactory clientFactory,
        IConfiguration configuration
    ) : BaseController(mapperService, clientFactory, configuration)
    {
        public override UriBuilder CloneApiUrl()
        {
            return base.CloneApiUrl().ExtendPath("Account");
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
                using var client = GetClientFactory();
                var payload = new { RegisterUserDto = userDto, };
                var jsonUserDto = new StringContent(
                    JsonConvert.SerializeObject(payload),
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
                            TempData["Error"] = error.Description;
                        }
                    }

                    TempData["Success"] = "Account successfully created!";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync();
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

                var payload = new { LoginUserDto = loginDto, };
                var jsonLoginDto = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                using var client = GetClientFactory();

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

                    HttpContext.Session.SetString("JwtToken", loginFeedbackDto.Token);

                    TempData["Success"] = "Login successful!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync();
                    return View(loginVM);
                }
            }
            return View(loginVM);
        }

        [ServiceFilter(typeof(JwtSessionAuthenticationAttribute))]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
