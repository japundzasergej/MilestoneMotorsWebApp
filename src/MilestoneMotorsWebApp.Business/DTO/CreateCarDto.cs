using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public record CreateCarDto
    {
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

        public byte[]? HeadlinerImageUrl { get; init; }

        public byte[]? PhotoOne { get; init; }

        public byte[]? PhotoTwo { get; init; }

        public byte[]? PhotoThree { get; init; }

        public byte[]? PhotoFour { get; init; }

        public byte[]? PhotoFive { get; init; }

        public string? UserId { get; set; }

        public string? AdNumber { get; init; }

        public DateTime CreatedAt { get; set; }

        public List<string> ImageContentTypes { get; set; }
    }
}
