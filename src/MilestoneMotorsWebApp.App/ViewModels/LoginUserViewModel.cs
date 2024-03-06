using System.ComponentModel.DataAnnotations;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
