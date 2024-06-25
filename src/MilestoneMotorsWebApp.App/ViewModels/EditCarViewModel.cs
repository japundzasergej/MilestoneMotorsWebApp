using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record EditCarViewModel
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
        public string Mileage { get; init; }

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
        public string EnginePower { get; init; }

        [Required]
        [DisplayName("Fixed Price")]
        public YesOrNo FixedPrice { get; init; }

        [Required]
        public YesOrNo Exchange { get; init; }
        public string? UserId { get; init; }
    }
}
