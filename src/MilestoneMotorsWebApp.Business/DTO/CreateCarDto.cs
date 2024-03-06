using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public class CreateCarDto
    {
        public Condition Condition { get; set; }

        public Brand Brand { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }

        public string Model { get; set; }

        public int ManufacturingYear { get; set; }

        public string Mileage { get; set; }

        public BodyTypes BodyTypes { get; set; }

        public FuelTypes FuelTypes { get; set; }

        public string EngineCapacity { get; set; }

        public string EnginePower { get; set; }

        public YesOrNo FixedPrice { get; set; }

        public YesOrNo Exchange { get; set; }

        public byte[]? HeadlinerImageUrl { get; set; }

        public byte[]? PhotoOne { get; set; }

        public byte[]? PhotoTwo { get; set; }

        public byte[]? PhotoThree { get; set; }

        public byte[]? PhotoFour { get; set; }

        public byte[]? PhotoFive { get; set; }

        public string? UserId { get; set; }

        public string? AdNumber { get; set; }

        public List<string> ImageContentTypes { get; set; }
    }
}
