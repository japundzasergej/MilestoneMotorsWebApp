using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.ViewModels;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(IAccountCommand accountCommand) : Controller
    {
        private readonly IAccountCommand _accountCommand = accountCommand;

        [HttpGet]
        public IActionResult Register()
        {
            var registerVM = _accountCommand.GetRegisterUser();
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerVM)
        {
            object onUserExists() => TempData["Error"] = "User already exists";
            object errorHandling() => TempData["Success"] = "Account successfully created!";
            if (ModelState.IsValid)
            {
                var user = await _accountCommand.PostRegisterUser(
                    registerVM,
                    onUserExists,
                    errorHandling
                );
                if (user)
                {
                    TempData["Success"] = "Account successfully created!";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return View(registerVM);
                }
            }
            return View(registerVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginVM = _accountCommand.GetLoginUser();
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginVM)
        {
            object onPasswordsNotMatching() => TempData["Error"] = "Invalid password.";
            object onInvalidUser() => TempData["Error"] = "No user registered with that email.";
            if (ModelState.IsValid)
            {
                var user = await _accountCommand.PostLoginUser(
                    loginVM,
                    onPasswordsNotMatching,
                    onInvalidUser
                );
                if (user)
                {
                    TempData["Success"] = "Login successful!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(loginVM);
                }
            }
            return View(loginVM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountCommand.LogoutUser();
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
    }
}
