using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record CreateCarViewModel
    {
        [Required]
        public Condition Condition { get; init; }

        [Required]
        [DisplayName("Car Brand")]
        public Brand Brand { get; init; }

        [Required]
        [MaxLength(150, ErrorMessage = "Max length of 100 characters")]
        public string Description { get; init; }

        [Required]
        public double Price { get; init; }

        [Required]
        [DisplayName("Car Model")]
        public string Model { get; init; }

        [Required]
        [DisplayName("Manufacturing Year")]
        [Range(typeof(int), "1920", "2024", ErrorMessage = "Invalid Manufacturing Year")]
        public int ManufacturingYear { get; init; }

        [Required]
        public int Mileage { get; init; }

        [Required]
        [DisplayName("Body Type")]
        public BodyTypes BodyTypes { get; init; }

        [Required]
        [DisplayName("Fuel Types")]
        public FuelTypes FuelTypes { get; init; }

        [Required]
        [DisplayName("Engine Capacity")]
        public int EngineCapacity { get; init; }

        [Required]
        [DisplayName("Engine Power")]
        public int EnginePower { get; init; }

        [Required]
        [DisplayName("Fixed Price")]
        public YesOrNo FixedPrice { get; init; }

        [Required]
        public YesOrNo Exchange { get; init; }

        [Required]
        [DisplayName("Headliner Image")]
        public IFormFile? HeadlinerImageUrl { get; init; }

        [Required(ErrorMessage = "Image required")]
        public IFormFile? PhotoOne { get; init; }

        [Required(ErrorMessage = "Image required")]
        public IFormFile? PhotoTwo { get; init; }

        [Required(ErrorMessage = "Image required")]
        public IFormFile? PhotoThree { get; init; }

        public IFormFile? PhotoFour { get; init; }

        public IFormFile? PhotoFive { get; init; }
        public string? UserId { get; init; }
        public string? AdNumber { get; init; }
    }
}
