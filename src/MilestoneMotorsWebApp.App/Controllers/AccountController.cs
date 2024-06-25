using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(IAccountService accountService, IMapper mapper) : BaseController
    {
        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerVM)
        {
            if (ModelState.IsValid)
            {
                var response = await accountService.RegisterUser(
                    mapper.Map<RegisterUserDto>(registerVM)
                );

                if (response.UserExists)
                {
                    TempData["Error"] = "User already exists with that username.";
                    return View(registerVM);
                }

                if (response.ResponseFailed)
                {
                    foreach (var error in response.ErrorList)
                    {
                        TempData["Error"] = error.Description;
                        return View(registerVM);
                    }
                }
                TempData["Success"] = "Account successfully created!";
                return RedirectToAction("Login", "Account");
            }
            return View(registerVM);
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
                var response = await accountService.LoginUser(mapper.Map<LoginUserDto>(loginVM));

                if (!response.IsValidUser)
                {
                    TempData["Error"] = "No user registered with that email.";
                    return View(loginVM);
                }

                if (response.IsNotPasswordsMatching)
                {
                    TempData["Error"] = "Invalid password.";
                    return View(loginVM);
                }

                HttpContext.Session.SetString("JwtToken", response.Token);

                TempData["Success"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }
            return View(loginVM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
