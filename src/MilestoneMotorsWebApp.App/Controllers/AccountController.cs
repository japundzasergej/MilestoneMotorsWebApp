using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MilestoneMotorsWebApp.App.Attributes;
using MilestoneMotorsWebApp.App.Controllers;
using MilestoneMotorsWebApp.App.Interfaces;
using MilestoneMotorsWebApp.App.ViewModels;
using MilestoneMotorsWebApp.Business.DTO;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.Utilities;
using Newtonsoft.Json;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(IAccountService accountService)
        : BaseController<IAccountService>(accountService)
    {
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
                var registerFeedbackDto = await _service.RegisterUser(registerVM);

                if (registerFeedbackDto == null)
                {
                    TempData["Error"] = "Something went wrong, please try again later.";
                    return View(registerVM);
                }

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
                var loginFeedbackDto = await _service.LoginUser(loginVM);

                if (loginFeedbackDto == null)
                {
                    TempData["Error"] = "Something went wrong, please try again later.";
                    return View(loginVM);
                }

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
