using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.App.ViewModels
{
    public record GetCarViewModel
    {
        public Condition Condition { get; init; }
        public Brand Brand { get; init; }
        public string Description { get; init; }
        public string Price { get; init; }
        public string Model { get; init; }
        public int ManufacturingYear { get; init; }
        public string Mileage { get; init; }
        public BodyTypes BodyTypes { get; init; }
        public FuelTypes FuelTypes { get; init; }
        public string EngineCapacity { get; init; }
        public string EnginePower { get; init; }
        public YesOrNo FixedPrice { get; init; }
        public YesOrNo Exchange { get; init; }
        public string? HeadlinerImageUrl { get; init; }
        public List<string>? ImagesUrl { get; init; }
        public string? AdNumber { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
