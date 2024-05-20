using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public record CarDto
    {
        public int Id { get; init; }
        public Condition Condition { get; init; }
        public Brand Brand { get; init; }
        public string Description { get; init; }
        public double Price { get; init; }
        public string Model { get; init; }
        public int ManufacturingYear { get; init; }
        public int Mileage { get; init; }
        public BodyTypes BodyTypes { get; init; }
        public FuelTypes FuelTypes { get; init; }
        public int EngineCapacity { get; init; }
        public int EnginePower { get; init; }
        public YesOrNo FixedPrice { get; init; }
        public YesOrNo Exchange { get; init; }
        public string? HeadlinerImageUrl { get; init; }
        public List<string>? ImagesUrl { get; init; }
        public string? AdNumber { get; init; }
        public DateTime CreatedAt { get; init; }
        public string? UserId { get; init; }
        public User? User { get; init; }
    }
}
