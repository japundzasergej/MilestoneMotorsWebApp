using System.ComponentModel.DataAnnotations;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record LoginUserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
