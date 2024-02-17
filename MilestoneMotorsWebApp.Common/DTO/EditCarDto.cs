using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class EditCarDto
    {
        public int Id { get; set; }
        public Condition Condition { get; set; }

        public Brand Brand { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Model { get; set; }

        public int ManufacturingYear { get; set; }

        public string Mileage { get; set; }

        public BodyTypes BodyTypes { get; set; }

        public FuelTypes FuelTypes { get; set; }

        public int EngineCapacity { get; set; }

        public string EnginePower { get; set; }

        public YesOrNo FixedPrice { get; set; }

        public YesOrNo Exchange { get; set; }
        public string? UserId { get; set; }

        public bool IsSuccessful { get; set; } = true;
    }
}
