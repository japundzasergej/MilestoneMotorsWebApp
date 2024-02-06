using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Interfaces;
using MilestoneMotorsWebApp.Business.ViewModels;
using MilestoneMotorsWebApp.Domain.Constants;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Accounts
{
    public class AccountCommand(UserManager<User> userManager, SignInManager<User> signInManager)
        : IAccountCommand
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        public LoginUserViewModel GetLoginUser()
        {
            return new LoginUserViewModel();
        }

        public RegisterUserViewModel GetRegisterUser()
        {
            return new RegisterUserViewModel();
        }

        public async Task<bool> PostRegisterUser(
            RegisterUserViewModel registerVM,
            Func<object> onUserExists,
            Func<object> errorHandling
        )
        {
            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if (user != null)
            {
                onUserExists();
                return false;
            }
            var newUser = new User
            {
                Email = registerVM.Email.Trim(),
                UserName = registerVM.Username.Trim()
            };
            var response = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (response.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return true;
            }
            else
            {
                foreach (var error in response.Errors)
                {
                    errorHandling();
                }

                return false;
            }
        }

        public async Task<bool> PostLoginUser(
            LoginUserViewModel loginVM,
            Func<object> onPasswordsNotMatching,
            Func<object> onInvalidUser
        )
        {
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(
                        user,
                        loginVM.Password,
                        false,
                        false
                    );
                    if (result.Succeeded)
                    {
                        return true;
                    }
                }
                onPasswordsNotMatching();
                return false;
            }
            onInvalidUser();
            return false;
        }

        public async Task<bool> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
