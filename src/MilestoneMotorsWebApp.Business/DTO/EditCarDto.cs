using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public record EditCarDto
    {
        public int Id { get; set; }
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
        public string? UserId { get; set; }
    }
}
