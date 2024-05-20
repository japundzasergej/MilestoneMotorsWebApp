using System.ComponentModel.DataAnnotations;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record LoginUserViewModel
    {
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}
