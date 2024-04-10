using System.Text;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.Models;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(IAccountService accountService, IMvcMapperService mapperService)
        : BaseController<IAccountService>(accountService, mapperService)
    {
        [HttpGet]
        public IActionResult Register()
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));

            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.RegisterUser(
                    _mapperService.Map<RegisterUserViewModel, RegisterUserDto>(registerVM)
                );

                var errorCheck = HandleErrors(
                    response,
                    new FailureResponse { ViewModel = registerVM }
                );

                if (errorCheck != null)
                {
                    return errorCheck;
                }

                if (response.Body != null)
                {
                    var registerFeedbackDto = ConvertFromJson<RegisterUserFeedbackDto>(
                        response.Body
                    );

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
                            return View(registerVM);
                        }
                    }
                    TempData["Success"] = "Account successfully created!";
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginVM)
        {
            var isAuthenticated = !string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken"));
            if (isAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var response = await _service.LoginUser(
                    _mapperService.Map<LoginUserViewModel, LoginUserDto>(loginVM)
                );

                var error = HandleErrors(response, new FailureResponse { ViewModel = loginVM });

                if (error != null)
                {
                    return error;
                }

                if (response.Body != null)
                {
                    var loginFeedbackDto = ConvertFromJson<LoginUserFeedbackDto>(response.Body);

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
